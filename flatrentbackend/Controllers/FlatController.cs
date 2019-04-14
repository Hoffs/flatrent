using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Attributes;
using FlatRent.Constants;
using FlatRent.Controllers.Abstractions;
using FlatRent.Controllers.Filters;
using FlatRent.Dtos;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Models.Requests;
using FlatRent.Models.Requests.Flat;
using FlatRent.Models.Responses;
using FlatRent.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(typeof(CreatedFlat), 201)]
        public async Task<IActionResult> CreateFlat(FlatForm form)
        {
//            if (form.Images.Any(i => !i.IsImage())) return BadRequest(new FormError("Images", Errors.InvalidImage));

            var (errors, flatId) = await _flatRepository.AddFlatAsync(form, HttpContext.User.GetUserId()).ConfigureAwait(false);
            if (errors != null) return BadRequest(errors);

            var imageIds = (await _flatRepository.GetAsync(flatId)).Images.Select(i => new KeyValuePair<string,Guid>(i.Name, i.Id));
            return StatusCode(201, new CreatedFlat(flatId, imageIds));
        }

        [Authorize(Policy = "User")]
        [HttpDelete("{id}")]
        [MustBeEntityAuthor]
        public async Task<IActionResult> DeleteFlat([FromRoute] Guid id)
        {
            var errors = await _flatRepository.DeleteAsync(id).ConfigureAwait(false);
            return HandleFormErrors(errors, 201);
        }

        [Authorize(Policy = "User")]
        [HttpPut("{id}")]
        [MustBeEntityAuthor]
        public async Task<IActionResult> UpdateFlat([FromRoute] Guid id, FlatForm form)
        {
            var actionResult = await IsAllowedToEditEntity(id, "FlatId");
            if (actionResult != null) return actionResult;

            throw new NotImplementedException();
        }

        [Authorize(Policy = "User")]
        [HttpPost("{id}/rent")]
        [EntityMustExist]
        public async Task<IActionResult> ApplyForRent([FromRoute] Guid id, [FromBody] RentAgreementForm form)
        {
            var errors = new List<FormError>();

            // TODO: Move to Business Rule

            var rentPeriod = TimeSpan.FromTicks(form.To.Ticks - form.From.Ticks).Days;
            if (rentPeriod < BusinessConstants.MinRentPeriodDays)
            {
                errors.Add(new FormError("To", string.Format(Errors.FlatRentPeriodGreater, BusinessConstants.MinRentPeriodDays)));
                return BadRequest(  );
            }

            if (rentPeriod > BusinessConstants.MaxRentPeriodDays)
            {
                errors.Add(new FormError("To", string.Format(Errors.FlatRentPeriodLess, BusinessConstants.MaxRentPeriodDays)));
                return BadRequest(errors.GetFormattedResponse());
            }

            // TODO: Move to BR
            var flat = await _flatRepository.GetAsync(id);
            if (flat.IsRented || !flat.IsPublished)
            {
                return BadRequest(new FormError(Errors.FlatNotAvailableForRent));
            }

            // TODO: Move to BR

            if (HttpContext.User.GetUserId() == flat.AuthorId)
            {
                return BadRequest(new FormError(Errors.TenantCantBeOwner));
            }

            var operationErrors = await _agreementRepository.CreateAgreementTask(id, HttpContext.User.GetUserId(), form).ConfigureAwait(false);

            var formErrors = operationErrors as FormError[] ?? operationErrors.ToArray();
            if (formErrors.Any())
            {
                return BadRequest(formErrors);
            }

            return Ok();
        }

        [Authorize]
        [ExactQueryParam("count", "offset")]
        [HttpGet]
        public async Task<IActionResult> GetFlats([Range(1, 100, ErrorMessage = Errors.Range)] int count = 20, [Range(0, int.MaxValue, ErrorMessage = Errors.Range)] int offset = 0)
        {
            var flats = await _flatRepository.GetListAsync(false, count, offset).ConfigureAwait(false);
            var mappedFlats = _mapper.Map<List<FlatListItem>>(flats);
            Response.Headers.Add("X-Total-Count", (await _flatRepository.GetCountAsync().ConfigureAwait(false)).ToString());
            return new OkObjectResult(mappedFlats);
        }

        [Authorize(Policy = "User")]
        [ExactQueryParam("rented", "count", "offset")]
        [HttpGet]
        public async Task<IActionResult> GetFlats(bool rented = false, [Range(1, 100)] int count = 20, [Range(0, int.MaxValue)] int offset = 0)
        {
            var flats = await _flatRepository.GetListAsync(rented, count, offset).ConfigureAwait(false);
            var mappedFlats = _mapper.Map<List<FlatListItem>>(flats);
            Response.Headers.Add("X-Total-Count", (await _flatRepository.GetCountAsync(rented).ConfigureAwait(false)).ToString());
            return new OkObjectResult(mappedFlats);
        }

        [Authorize]
        [HttpGet("{id}")]
        [EntityMustExist]
        public async Task<IActionResult> GetFlat([FromRoute] Guid id)
        {
            var flat = await _flatRepository.GetAsync(id).ConfigureAwait(false);
            if (!flat.IsPublished && flat.AuthorId != HttpContext.User.GetUserId()) return NotFound(id); // Not published can be seen only by author
            return new OkObjectResult(flat);
        }
    }
}