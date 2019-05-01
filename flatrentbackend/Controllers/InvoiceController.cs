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
using FlatRent.Repositories.Interfaces;
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
        private readonly ILogger _logger;

        public InvoiceController(IAgreementRepository repository, IInvoiceRepository invoiceRepository, IMapper mapper, ILogger logger) : base(repository)
        {
            _repository = repository;
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> GetInvoicesAsync([FromRoute] Guid id)
        {
            var agreement = await _repository.GetAsync(id);
            var userId = User.GetUserId();
            if (userId != agreement.TenantId && userId != agreement.Flat.AuthorId) return Forbid();

            var mapped = _mapper.Map<IEnumerable<InvoiceDetails>>(agreement.Invoices);
            return Ok(mapped);
        }

        [HttpGet("{invoiceId}")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> GetInvoiceAsync([FromRoute] Guid id, [FromRoute] Guid invoiceId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{invoiceId}/pdf")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> GetInvoicePDFAsync([FromRoute] Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{invoiceId}/pay")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> PayInvoiceAsync([FromRoute] Guid id, [FromRoute] Guid invoiceId)
        {
            var agreement = await _repository.GetAsync(id);
            var invoice = agreement.Invoices.FirstOrDefault(inv => inv.Id == invoiceId);
            if (invoice == null) return NotFound();
            if (!invoice.IsValid) return BadRequest(new FormError(Errors.InvoiceNotValid));

            // Just set it to paid, as no actual payment processing is done.
            invoice.IsPaid = true;
            var errors = await _invoiceRepository.UpdateInvoiceTask(invoice);
            return OkOrBadRequest(errors, Ok());
    }
}