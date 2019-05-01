using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Constants;
using FlatRent.Controllers.Abstractions;
using FlatRent.Controllers.Attributes;
using FlatRent.Controllers.Filters;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Models.Dtos;
using FlatRent.Models.Requests;
using FlatRent.Models.Requests.Flat;
using FlatRent.Models.Responses;
using FlatRent.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FlatRent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlatController : AuthoredBaseEntityController<Flat>
    {
        private readonly IFlatRepository _flatRepository;
        private readonly IAgreementRepository _agreementRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public FlatController(IFlatRepository flatRepository, IAgreementRepository agreementRepository, IMapper mapper, ILogger logger) : base(flatRepository)
        {
            _flatRepository = flatRepository;
            _agreementRepository = agreementRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize(Policy = "User")]
        [HttpPost]
        [ProducesResponseType(typeof(CreatedFlatResponse), 201)]
        public async Task<IActionResult> CreateFlat(FlatForm form)
        {
//            if (form.Images.Any(i => !i.IsImage())) return BadRequest(new FormError("Images", Errors.InvalidImage));
            if (form.TotalFloors < form.Floor)
                return BadRequest(new FormError(nameof(form.TotalFloors), Errors.FloorCantBeHigherThanTotalFloors));

            var (errors, flatId) = await _flatRepository.AddFlatAsync(form, HttpContext.User.GetUserId()).ConfigureAwait(false);
            if (errors != null) return BadRequest(errors);

            var imageIds = (await _flatRepository.GetAsync(flatId)).Images.Select(i => new KeyValuePair<Guid, string>(i.Id, i.Name));
            return StatusCode(201, new CreatedFlatResponse(flatId, imageIds));
        }

        [Authorize(Policy = "User")]
        [HttpDelete("{id}")]
        [MustBeEntityAuthor]
        public async Task<IActionResult> DeleteFlat([FromRoute] Guid id)
        {
            var flat = await _flatRepository.GetAsync(id);
            if (flat.ActiveAgreement != null) return BadRequest(new FormError(Errors.CantDeleteWithActiveAgreements));
            var errors = await _flatRepository.DeleteAsync(id).ConfigureAwait(false);
            return HandleFormErrors(errors, 201);
        }

        [Authorize(Policy = "User")]
        [HttpPut("{id}")]
        [MustBeEntityAuthor]
        public async Task<IActionResult> UpdateFlat([FromRoute] Guid id, FlatForm form)
        {
            var actionResult = await IsEntityAuthor(id, "FlatId");
            if (actionResult != null) return actionResult;

            throw new NotImplementedException();
        }

        [Authorize(Policy = "User")]
        [HttpPost("{id}/rent")]
        [EntityMustExist]
        public async Task<IActionResult> ApplyForRent([FromRoute] Guid id, [FromBody] AgreementForm form)
        {
            var flat = await _flatRepository.GetAsync(id);

            // TODO: Move to Business Rule

            var rentPeriod = TimeSpan.FromTicks(form.To.Date.Ticks - form.From.Date.Ticks).Days;
            if (rentPeriod < flat.MinimumRentDays)
            {
                return BadRequest(new FormError("To", string.Format(Errors.FlatRentPeriodGreater, BusinessConstants.MinRentPeriodDays)));
            }

            if (rentPeriod > BusinessConstants.MaxRentPeriodDays)
            {
                return BadRequest(new FormError("To", string.Format(Errors.FlatRentPeriodLess, BusinessConstants.MaxRentPeriodDays)));
            }

            // TODO: Move to BR

            // flat.IsRented ||

            if (!flat.IsPublished || flat.ActiveAgreement != null)
            {
                return BadRequest(new FormError(Errors.FlatNotAvailableForRent));
            }

            // TODO: Move to BR

            if (User.GetUserId() == flat.AuthorId)
            {
                return BadRequest(new FormError(Errors.TenantCantBeOwner));
            }

            if (flat.Agreements.Any(Agreement.RequestedAgreementByUserFunc(User.GetUserId())))
            {
                return BadRequest(new FormError(Errors.AlreadyRequested));
            }

            var (operationErrors, agreement) = await _agreementRepository.AddAgreementAsync(id, HttpContext.User.GetUserId(), form).ConfigureAwait(false);
            return OkOrBadRequest(operationErrors,
                StatusCode(201, new CreatedAgreementResponse(agreement.Id, agreement.Attachments)));
        }

        [AllowAnonymous]
        [ExactQueryParam("count", "offset")]
        [HttpGet]
        public Task<IActionResult> GetFlats([Range(0, int.MaxValue, ErrorMessage = Errors.Range)] int offset = 0)
        {
            return GetFlats(false, offset);
        }

        [AllowAnonymous]
        [ExactQueryParam("rented", "count", "offset")]
        [HttpGet]
        public async Task<IActionResult> GetFlats(bool rented = false, [Range(0, int.MaxValue)] int offset = 0)
        {
            var flats = _flatRepository.GetListAsync(rented, 20, offset);
            var mappedFlats = _mapper.ProjectTo<ShortFlatDetails>(flats);
            Response.Headers.Add("X-Total-Count", (await _flatRepository.GetCountAsync(rented).ConfigureAwait(false)).ToString());
            await Task.Delay(1500);
            return new OkObjectResult(mappedFlats);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [EntityMustExist]
        public async Task<IActionResult> GetFlat([FromRoute] Guid id)
        {
            var flat = await _flatRepository.GetAsync(id).ConfigureAwait(false);
            if (!flat.IsPublished && flat.AuthorId != HttpContext.User.GetUserId()) return NotFound(id); // Not published can be seen only by author
            await Task.Delay(750);
            return new OkObjectResult(_mapper.Map<FlatDetails>(flat));
        }
    }
}