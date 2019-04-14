using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Constants;
using FlatRent.Controllers.Filters;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Files;
using FlatRent.Models;
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

        [HttpDelete("{id}")]
        [Authorize(Policy = "User")]
        [EntityMustExist]
//        [MustBeEntityAuthor] // Should be custom that allows for tenant and renter  to cancel
        public async Task<IActionResult> Cancel([FromRoute] Guid id)
        {
            // TODO: Overhaul, shouldn't be able to cancel this easily

            if (HttpContext.User.IsInRole(UserType.User.Role))
            {
                var agreement = await _repository.GetAsync(id).ConfigureAwait(false);

                // TODO: Maybe additional business rule that user can cancel if 30 days until agreement starts

                if (agreement.TenantId != HttpContext.User.GetUserId())
                {
                    return BadRequest(new FormError(Errors.AgreementCancelNotOwner));
                }
            }


            var errors = await _repository.CancelAgreement(id).ConfigureAwait(false);
            if (errors.Any())
            {
                return BadRequest(errors.GetFormattedResponse());
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

            if (HttpContext.User.IsInRole(UserType.User.Role))
            {
                if (agreement.TenantId != HttpContext.User.GetUserId())
                {
                    return BadRequest(new []{new FormError(Errors.AgreementPdfNotOwner)}.GetFormattedResponse());
                }
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