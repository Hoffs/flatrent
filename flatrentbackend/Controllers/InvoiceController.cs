﻿using System;
using System.Collections.Generic;
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
using FlatRent.Services.Interfaces;
using jsreport.Binary;
using jsreport.Local;
using jsreport.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FlatRent.Controllers
{
    [Controller]
    [Route("api/agreement/{id}/[controller]")]
    public class InvoiceController : AuthoredBaseEntityController<Agreement>
    {
        private readonly IAgreementRepository _repository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;

        public InvoiceController(IAgreementRepository repository, IInvoiceRepository invoiceRepository, IInvoiceService invoiceService, IMapper mapper, ILogger logger) : base(repository)
        {
            _repository = repository;
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> GetInvoicesAsync([FromRoute] Guid id, [FromQuery] int offset)
        {
            var agreement = await _repository.GetAsync(id);
            var userId = User.GetUserId();
            if (userId != agreement.AuthorId && userId != agreement.Flat.AuthorId) return Forbid();

            var mapped = _mapper.Map<IEnumerable<InvoiceDetails>>(agreement.Invoices.Paginate(offset));
            return Ok(mapped);
        }

        [HttpGet("{invoiceId}")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> GetInvoiceAsync([FromRoute] Guid id, [FromRoute] Guid invoiceId)
        {
            var agreement = await _repository.GetAsync(id);
            var userId = User.GetUserId();
            if (agreement.IsRenterOrTenant(userId)) return Forbid();
            var invoice = agreement.Invoices.FirstOrDefault(i => i.Id == invoiceId);

            var mapped = _mapper.Map<InvoiceDetails>(invoice);
            return Ok(mapped);
        }

        [HttpGet("{invoiceId}/pdf")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> GetInvoicePDFAsync([FromRoute] Guid id, [FromRoute] Guid invoiceId)
        {
            var agreement = await _repository.GetAsync(id).ConfigureAwait(false);
            var userId = User.GetUserId();

            if (agreement.AuthorId != userId && agreement.Flat.AuthorId != userId)
            {
                return Forbid();
            }

            var invoice = agreement.Invoices.FirstOrDefault(i => i.Id == invoiceId);
            if (invoice == null) return NotFound();

            var rows = invoice.Incidents.Select(f => $@"
            <tr>
                <td>Incidentas: {f.Name}</td>
                <td class=""right"">{f.Price}</td>            
            </tr>
            ");
            var incidentPrice = invoice.Incidents.Sum(f => f.Price);
            var patchData = new InvoicePatchData
            {
                Year = invoice.CreatedDate.Year,
                Month = invoice.CreatedDate.Month,
                Day = invoice.CreatedDate.Day,
                Price = invoice.AmountToPay - incidentPrice,
                AgreementNo = id,
                InvoiceFrom = invoice.InvoicedPeriodFrom.ToString("yyyy-MM-dd"),
                InvoiceTo = invoice.InvoicedPeriodTo.ToString("yyyy-MM-dd"),
                InvoiceDue = invoice.DueDate.ToString("yyyy-MM-dd"),
                InvoiceNo = invoiceId,
                TotalPrice = invoice.AmountToPay,
                AdditionalRows = string.Join("", rows),
            };

            var html = await HtmlGenerator.GetInvoiceHtml(patchData).ConfigureAwait(false);

            var rs = new LocalReporting().UseBinary(JsReportBinary.GetBinary()).AsUtility().Create();
            var report = await rs.RenderAsync(new RenderRequest
            {
                Template = new Template
                {
                    Recipe = Recipe.ChromePdf,
                    Engine = Engine.None,
                    Content = html,
                }
            }).ConfigureAwait(false);

            return File(report.Content, "application/pdf", "invoice.pdf");

        }

        [HttpPost("{invoiceId}/pay")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> PayInvoiceAsync([FromRoute] Guid id, [FromRoute] Guid invoiceId)
        {
            var agreement = await _repository.GetAsync(id);
            if (User.GetUserId() != agreement.AuthorId) return Forbid();
            var invoice = agreement.Invoices.FirstOrDefault(inv => inv.Id == invoiceId);
            if (invoice == null) return NotFound();
            if (!invoice.IsValid) return BadRequest(new FormError(Errors.InvoiceNotValid));
            if (invoice.IsPaid) return BadRequest(new FormError(Errors.InvoiceAlreadyPaid));

            // Just set it to paid, as no actual payment processing is done.
            invoice.IsPaid = true;
            invoice.IsValid = false;
            var errors = await _invoiceRepository.UpdateInvoiceTask(invoice);
            return OkOrBadRequest(errors, Ok(new { Id = id }));
        }
    }
}