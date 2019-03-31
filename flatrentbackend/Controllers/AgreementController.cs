using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Constants;
using FlatRent.Extensions;
using FlatRent.Files;
using FlatRent.Interfaces;
using FlatRent.Models;
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
    public class AgreementController : Controller
    {
        private readonly IAgreementRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AgreementController(IAgreementRepository repository, IMapper mapper, ILogger logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Cancel([FromRoute] Guid id)
        {
            // TODO: Overhaul, shouldn't be able to cancel this easily
            var payload = JwtPayload.CreateFromClaims(HttpContext.User.Claims);
            if (payload.UserType == "User")
            {
                var agreement = await _repository.GetAgreement(id).ConfigureAwait(false);
                if (agreement.RenterId != HttpContext.User.GetUserId())
                {
                    return BadRequest(new []{new FormError(Errors.AgreementCancelNotOwner)});
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
        public async Task<IActionResult> GetPdf([FromRoute] Guid id)
        {
            var agreement = await _repository.GetAgreement(id).ConfigureAwait(false);
            if (agreement == null)
            {
                return NotFound(new []{new FormError(Errors.AgreementNotFound)});
            }

            var payload = JwtPayload.CreateFromClaims(HttpContext.User.Claims);
            if (payload.UserType == "Client")
            {
                if (agreement.RenterId != HttpContext.User.GetUserId())
                {
                    return BadRequest(new []{new FormError(Errors.AgreementPdfNotOwner)});
                }
            }

            var agreementData = new AgreementPatchData
            {
                Year = agreement.CreatedDate.Year,
                Month = agreement.CreatedDate.Month,
                Day = agreement.CreatedDate.Day,
                Address = agreement.Flat.Address.ToString(),
                Owner = agreement.Flat.Owner.GetFullName(),
                Area = agreement.Flat.Area,
                Price = agreement.Flat.Price,
                AgreementNo = agreement.Id,
                Client = agreement.Renter.GetFullName(),
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