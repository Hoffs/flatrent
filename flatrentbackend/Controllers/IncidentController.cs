using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.BusinessRules;
using FlatRent.Constants;
using FlatRent.Controllers.Abstractions;
using FlatRent.Controllers.Filters;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Models.Dtos;
using FlatRent.Models.Requests;
using FlatRent.Models.Responses;
using FlatRent.Repositories.Interfaces;
using FlatRent.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FlatRent.Controllers
{
    [Controller]
    [Route("api/agreement/{id}/[controller]")]
    public class IncidentController : AuthoredBaseEntityController<Agreement>
    {
        private readonly IAgreementRepository _repository;
        private readonly IIncidentRepository _incidentRepository;
        private readonly IIncidentService _incidentService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public IncidentController(IAgreementRepository repository, IIncidentRepository incidentRepository, IIncidentService incidentService, IMapper mapper, ILogger logger) : base(repository)
        {
            _repository = repository;
            _incidentRepository = incidentRepository;
            _incidentService = incidentService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        [MustBeEntityAuthor]
        public async Task<IActionResult> CreateIncidentAsync([FromRoute] Guid id, [FromBody] IncidentForm form)
        {
            var agreement = await _repository.GetAsync(id);
            if (User.GetUserId() != agreement.TenantId) return Forbid();

            var (passed, error) = IncidentRules.AgreementMustBeActive(agreement);
            if (!passed) return BadRequest(error);

            var (errors, incident) = await _incidentRepository.CreateAsync(id, form, User.GetUserId());
            if (errors == null)
            {
                var loadedIncident = await _incidentRepository.GetLoadedAsync(incident.Id);
                await _incidentService.SendIncidentEmail(loadedIncident);
            }
            return OkOrBadRequest(errors, Ok(new CreatedIncidentResponse(incident.Id, incident.Attachments)));
        }

        [HttpGet]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> GetIncidentAsync([FromRoute] Guid id, [Required] int offset)
        {
            var agreement = await _repository.GetAsync(id);
            var userId = User.GetUserId();
            if (userId != agreement.TenantId && userId != agreement.Flat.AuthorId) return Forbid();

            var mapped = _mapper.Map<IEnumerable<ShortIncidentDetails>>(agreement.Incidents.Where(f => !f.Deleted).Paginate(offset));
            return Ok(mapped);
        }

        [HttpGet("{incidentId}")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> GetIncidentAsync([FromRoute] Guid id, [FromRoute] Guid incidentId)
        {
            var agreement = await _repository.GetAsync(id);
            var userId = User.GetUserId();
            if (userId != agreement.TenantId && userId != agreement.Flat.AuthorId) return Forbid();

            var incident = agreement.Incidents.FirstOrDefault(f => f.Id == incidentId);
            if (incident == null) return NotFound();

            var mapped = _mapper.Map<IncidentDetails>(incident);
            return Ok(mapped);
        }

        [HttpPost("{incidentId}/fixed")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> UpdateIncidentAsync([FromRoute] Guid id, [FromRoute] Guid incidentId, [FromBody] IncidentFixForm form)
        {
            var agreement = await _repository.GetAsync(id);

            if (User.GetUserId() != agreement.Flat.AuthorId) return Forbid();

            var incident = agreement.Incidents.FirstOrDefault(x => x.Id == incidentId);
            if (incident == null) return NotFound();

            incident.Repaired = true;
            incident.Price = form.Price;

            var errors = await _incidentRepository.UpdateAsync(incident);
            return OkOrBadRequest(errors, Ok(new { Id = incidentId }));
        }

        [HttpDelete("{incidentId}")]
        [Authorize]
        [MustBeEntityAuthor]
        public async Task<IActionResult> DeleteIncidentAsync([FromRoute] Guid id, [FromRoute] Guid incidentId)
        {
            var agreement = await _repository.GetAsync(id);
            var incident = agreement.Incidents.FirstOrDefault(x => x.Id == incidentId);
            if (incident == null) return NotFound();
            if (incident.Repaired) return BadRequest(new FormError(Errors.CantDeleteRepaired));

            var errors = await _incidentRepository.DeleteAsync(incident);
            return OkOrBadRequest(errors, Ok(new { Id = incidentId }));
        }
    }
}