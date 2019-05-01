using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Constants;
using FlatRent.Controllers.Abstractions;
using FlatRent.Controllers.Filters;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Models.Dtos;
using FlatRent.Models.Requests;
using FlatRent.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FlatRent.Controllers
{
    [Controller]
    [Route("api/agreement/{id}/[controller]")]
    public class FaultController : AuthoredBaseEntityController<Agreement>
    {
        private readonly IAgreementRepository _repository;
        private readonly IFaultRepository _faultRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public FaultController(IAgreementRepository repository, IFaultRepository faultRepository, IMapper mapper, ILogger logger) : base(repository)
        {
            _repository = repository;
            _faultRepository = faultRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        [MustBeEntityAuthor]
        public async Task<IActionResult> CreateFaultAsync([FromRoute] Guid id, [FromBody] FaultForm form)
        {
            var agreement = await _repository.GetAsync(id);
            if (User.GetUserId() != agreement.TenantId) return Forbid();
            if (agreement.Status.Id != AgreementStatus.Statuses.Accepted) return BadRequest();

            var errors = await _faultRepository.CreateFaultAsync(id, form, User.GetUserId());
            return OkOrBadRequest(errors, Ok());
        }

        [HttpGet]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> GetFaultsAsync([FromRoute] Guid id)
        {
            var agreement = await _repository.GetAsync(id);
            var userId = User.GetUserId();
            if (userId != agreement.TenantId && userId != agreement.Flat.AuthorId) return Forbid();

            var mapped = _mapper.Map<IEnumerable<ShortFaultDetails>>(agreement.Faults);
            return Ok(mapped);
        }

        [HttpGet("{faultId}")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> GetFaultAsync([FromRoute] Guid id, [FromRoute] Guid faultId)
        {
            var agreement = await _repository.GetAsync(id);
            var userId = User.GetUserId();
            if (userId != agreement.TenantId && userId != agreement.Flat.AuthorId) return Forbid();

            var fault = agreement.Faults.FirstOrDefault(f => f.Id == faultId);
            if (fault == null) return NotFound();

            var mapped = _mapper.Map<FaultDetails>(fault);
            return Ok(mapped);
        }

        [HttpPost("{faultId}/fixed")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> UpdateFaultAsync([FromRoute] Guid id, [FromRoute] Guid faultId, [FromBody] float price)
        {
            var agreement = await _repository.GetAsync(id);

            if (User.GetUserId() != agreement.Flat.AuthorId) return Forbid();

            var fault = agreement.Faults.FirstOrDefault(x => x.Id == faultId);
            if (fault == null) return NotFound();

            fault.Repaired = true;
            fault.Price = price;

            var errors = await _faultRepository.UpdateAsync(fault);
            return OkOrBadRequest(errors, Ok());
        }

        [HttpDelete("{faultId}")]
        [Authorize]
        [MustBeEntityAuthor]
        public async Task<IActionResult> DeleteFaultAsync([FromRoute] Guid id, [FromRoute] Guid faultId)
        {
            var agreement = await _repository.GetAsync(id);
            var fault = agreement.Faults.FirstOrDefault(x => x.Id == faultId);
            if (fault == null) return NotFound();
            if (fault.Repaired) return BadRequest(new FormError(Errors.CantDeleteRepaired));

            var errors = await _faultRepository.DeleteAsync(fault);
            return OkOrBadRequest(errors, Ok());
        }
    }
}