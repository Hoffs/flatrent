﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.BusinessRules;
using FlatRent.BusinessRules.Builder;
using FlatRent.BusinessRules.Builder.Extensions;
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
using FlatRent.Services;
using FlatRent.Services.Interfaces;
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
        private readonly IAgreementService _agreementService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public FlatController(IFlatRepository flatRepository, IAgreementRepository agreementRepository, IAgreementService agreementService, IMapper mapper, ILogger logger) : base(flatRepository)
        {
            _flatRepository = flatRepository;
            _agreementRepository = agreementRepository;
            _agreementService = agreementService;
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
        public async Task<IActionResult> UpdateFlat([FromRoute] Guid id, FlatUpdateForm form)
        {
            var (errors, newImages) = await _flatRepository.UpdateAsync(id, form);
            if (errors != null) return BadRequest(errors);

            var imageIds = newImages.Select(i => new KeyValuePair<Guid, string>(i.Id, i.Name));
            return StatusCode(201, new CreatedFlatResponse(id, imageIds));
        }

        [Authorize(Policy = "User")]
        [HttpPost("{id}/rent")]
        [EntityMustExist]
        public async Task<IActionResult> ApplyForRent([FromRoute] Guid id, [FromBody] AgreementForm form)
        {
            var flat = await _flatRepository.GetAsync(id);

//            var (passed, error) = RuleChecker.Check(
//                () => AgreementRequestRules.PeriodMustBeLongerOrEqualToSpecified(flat, form),
//                () => AgreementRequestRules.PeriodMustBeShorterOrEqualToMaximum(flat, form),
//                () => AgreementRequestRules.FlatMustHaveAtMostOneActiveAgreement(flat),
//                () => AgreementRequestRules.TenantCantBeOwner(flat, User.GetUserId())
//            );
//
//            if (!passed)
//            {
//                return BadRequest(error);
//            }

            // TODO: Move to BR
            if (flat.Agreements.Any(Agreement.RequestedAgreementByUserFunc(User.GetUserId())))
            {
                return BadRequest(new FormError(Errors.AlreadyRequested));
            }

            var (operationErrors, agreement) = await _agreementRepository.AddAgreementAsync(id, HttpContext.User.GetUserId(), form).ConfigureAwait(false);
            if (operationErrors == null)
            {
                var loadedAgreement = await _agreementRepository.GetLoadedAsync(agreement.Id);
                await _agreementService.SendNewAgreementEmailAsync(loadedAgreement);
            }
            return OkOrBadRequest(operationErrors,
                StatusCode(201, new CreatedAgreementResponse(agreement?.Id ?? Guid.Empty, agreement?.Attachments)));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetFlats([Range(0, int.MaxValue)] int offset = 0, [FromQuery] FlatListFilters filters = null)
        {
            var flats = _flatRepository.GetListAsync(20, offset, filters);
            var mappedFlats = _mapper.ProjectTo<ShortFlatDetails>(flats);
            return new OkObjectResult(mappedFlats);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [EntityMustExist]
        public async Task<IActionResult> GetFlat([FromRoute] Guid id)
        {
            var flat = await _flatRepository.GetAsync(id).ConfigureAwait(false);
            return new OkObjectResult(_mapper.Map<FlatDetails>(flat));
        }
    }
}