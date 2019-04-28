using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Constants;
using FlatRent.Controllers.Abstractions;
using FlatRent.Controllers.Filters;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Files;
using FlatRent.Models;
using FlatRent.Models.Dtos;
using FlatRent.Repositories.Interfaces;
using jsreport.Binary;
using jsreport.Local;
using jsreport.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FlatRent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgreementController : AuthoredBaseEntityController<Agreement>
    {
        private readonly IAgreementRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AgreementController(IAgreementRepository repository, IMapper mapper, ILogger logger) : base(repository)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> GetAgreementAsync([FromRoute] Guid id)
        {
            var agreement = await _repository.GetAsync(id);
            if (agreement.TenantId != User.GetUserId() && agreement.Flat.AuthorId != User.GetUserId())
            {
                return Forbid();
            }

            var mapped = _mapper.Map<AgreementDetails>(agreement);

            // Don't show email and phone if agreement wasn't accepted.
            // POSSIBLE BUSINESS RULE
            if (mapped.Status.Id != AgreementStatus.Statuses.Accepted)
            {
                mapped.Owner.Email = "";
                mapped.Owner.PhoneNumber = "";
                mapped.Tenant.Email = "";
                mapped.Tenant.PhoneNumber = "";
            }
            return Ok(mapped);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "User")]
        [MustBeEntityAuthor]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var errors = await _repository.DeleteAsync(id).ConfigureAwait(false);
            if (errors != null)
            {
                return BadRequest(errors);
            }

            return Ok();
        }

        [HttpPost("{id}/accept")]
        [Authorize(Policy = "User")]
        [EntityMustExist]
        public async Task<IActionResult> AcceptAgreement([FromRoute] Guid id)
        {
            var agreement = await _repository.GetAsync(id);
            if (agreement.Flat.AuthorId != User.GetUserId()) return Forbid();

            agreement.StatusId = AgreementStatus.Statuses.Accepted;
            var errors = await _repository.UpdateAsync(agreement);
            if (errors != null)
            {
                return BadRequest(errors);
            }


            // Update other agreement to rejected.
            try
            {
                var requestedAgreements =
                    agreement.Flat.Agreements.Where(a => a.StatusId == AgreementStatus.Statuses.Requested).ToList();
                requestedAgreements.SetProperty(ra => ra.StatusId, AgreementStatus.Statuses.Rejected);
                foreach (var ra in requestedAgreements)
                {
                    await _repository.UpdateAsync(ra);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Exception thrown while updating other requested agreements.");
            }


            return Ok();
        }

        [HttpPost("{id}/reject")]
        [Authorize(Policy = "User")]
        [EntityMustExist]
        public async Task<IActionResult> RejectAgreement([FromRoute] Guid id)
        {
            var agreement = await _repository.GetAsync(id);
            if (agreement.Flat.AuthorId != User.GetUserId()) return Forbid();

            agreement.StatusId = AgreementStatus.Statuses.Rejected;
            var errors = await _repository.UpdateAsync(agreement);
            if (errors != null)
            {
                return BadRequest(errors);
            }

            return Ok();
        }

        [HttpGet("{id}/pdf")]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        [Authorize(Policy = "User")]
        [EntityMustExist]
        public async Task<IActionResult> GetPdf([FromRoute] Guid id)
        {
            var agreement = await _repository.GetAsync(id).ConfigureAwait(false);
            var userId = User.GetUserId();

            if (agreement.TenantId != userId && agreement.Flat.AuthorId != userId)
            {
                return Forbid();
            }

            var agreementData = new AgreementPatchData
            {
                Year = agreement.CreatedDate.Year,
                Month = agreement.CreatedDate.Month,
                Day = agreement.CreatedDate.Day,
                Address = agreement.Flat.Address.ToString(),
                Owner = agreement.Flat.Author.GetFullName(),
                Area = agreement.Flat.Area,
                Price = agreement.Flat.Price,
                AgreementNo = agreement.Id,
                Client = agreement.Tenant.GetFullName(),
            };

            var agreementHtml = await HtmlGenerator.GetAgreementHtml(agreementData).ConfigureAwait(false);

            var rs = new LocalReporting().UseBinary(JsReportBinary.GetBinary()).AsUtility().Create();
            var report = await rs.RenderAsync(new RenderRequest
            {
                Template = new Template
                {
                    Recipe = Recipe.ChromePdf,
                    Engine = Engine.None,
                    Content = agreementHtml,
                }
            }).ConfigureAwait(false);

            return File(report.Content, "application/pdf", "agreement.pdf");
        }
    }
}