using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Repositories.Interfaces;
using FlatRent.Services.Interfaces;
using Serilog;

namespace FlatRent.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IAgreementRepository _agreementRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;

        public InvoiceService(IAgreementRepository agreementRepository, IInvoiceRepository invoiceRepository, IEmailService emailService, ILogger logger)
        {
            _agreementRepository = agreementRepository;
            _invoiceRepository = invoiceRepository;
            _emailService = emailService;
            _logger = logger;
        }

        /// <summary>
        /// Generates initial invoice which has to be paid until the agreement starts.
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        public async Task GenerateInitialInvoiceAsync(Guid agreementId)
        {
            var agreement = await _agreementRepository.GetAsync(agreementId);
            if (agreement == null || agreement.Deleted || agreement.StatusId != AgreementStatus.Statuses.Accepted)
            {
                throw new ArgumentException("Agreement doesn't exist or is not accepted.", nameof(agreementId));
            }

            var invoice = new Invoice
            {
                AgreementId = agreementId,
                AmountToPay = agreement.Price,
                DueDate = agreement.From.Date,
                InvoicedPeriodFrom = agreement.From.Date,
                InvoicedPeriodTo = new DateTime(Math.Min(agreement.From.AddDays(BusinessConstants.RentMonthInDays).Date.Ticks, agreement.To.Date.Ticks)).Date,
                IsValid = true,
            };

            var errors = await _invoiceRepository.AddInvoiceTask(invoice);
            if (errors != null)
            {
                _logger.Error($"Received errors when generating invoice for agreement {agreementId}: {errors.GetFormattedResponse()}");
            }

            await SendInvoiceEmail(invoice);
        }

        /// <summary>
        /// Generates invoices for agreement by including unpaid 
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        public async Task GenerateInvoiceForAgreementAsync(Guid agreementId)
        {
            var agreement = await _agreementRepository.GetAsync(agreementId);
            if (agreement == null || agreement.Deleted || agreement.StatusId != AgreementStatus.Statuses.Accepted)
            {
                throw new ArgumentException("Agreement doesn't exist or is not accepted.", nameof(agreementId));
            }

            var lastInvoice = agreement.Invoices.OrderByDescending(i => i.CreatedDate).First();
            // Check if last invoice was last absolute last, if yes, don't generate another one.
            var notInvoicedIncidents = agreement.Incidents.Where(Incident.NotInvoicedIncidentsFunc).ToList();

            if (lastInvoice.InvoicedPeriodTo.Date == agreement.To.Date)
            {
                if (notInvoicedIncidents.Count > 0)
                {
                    await GenerateOnlyIncidentInvoiceAsync(agreement, notInvoicedIncidents);
                }
                return;
            }

            var invoicedPeriodStart = lastInvoice.InvoicedPeriodTo.Date.AddDays(1).Date;
            var invoicedPeriodEnd = invoicedPeriodStart.AddDays(BusinessConstants.RentMonthInDays).Date;
            var dueDate = invoicedPeriodStart.AddDays(BusinessConstants.PaymentDueInDays).Date;
            var amountToPay = agreement.Price;

            if (invoicedPeriodEnd > agreement.To.Date)
            {
                invoicedPeriodEnd = agreement.To.Date;
                dueDate = agreement.To.Date;

                var daysLeft = invoicedPeriodEnd.Subtract(invoicedPeriodStart.Date).Days;
                amountToPay = (float) Math.Round(agreement.Price * ((float) daysLeft / BusinessConstants.RentMonthInDays), 2, MidpointRounding.AwayFromZero);
            }

            // Calculate and add faults to invoice
            var faultPrice = notInvoicedIncidents.Sum(f => f.Price);

            // Add last invoice if it wasn't paid.
            if (!lastInvoice.IsPaid)
            {
                amountToPay += lastInvoice.AmountToPay;
            }
            lastInvoice.IsValid = false;

            var invoice = new Invoice
            {
                AgreementId = agreementId,
                AmountToPay = faultPrice + amountToPay,
                Incidents = notInvoicedIncidents,
                DueDate = dueDate,
                InvoicedPeriodFrom = invoicedPeriodStart,
                InvoicedPeriodTo = invoicedPeriodEnd,
                IsValid = true,
            };

            var errors = await _invoiceRepository.AddAndUpdateTask(invoice, lastInvoice);
            if (errors != null)
            {
                _logger.Error($"Received errors when generating invoice for agreement {agreementId}: {errors.GetFormattedResponse()}");
            }

            await SendInvoiceEmail(invoice);
        }

        private async Task GenerateOnlyIncidentInvoiceAsync(Agreement agreement, List<Incident> notInvoicedIncidents)
        {
            // Calculate and add faults to invoice
            var faultPrice = notInvoicedIncidents.Sum(f => f.Price);

            var invoice = new Invoice
            {
                AgreementId = agreement.Id,
                AmountToPay = faultPrice,
                Incidents = notInvoicedIncidents,
                DueDate = DateTime.Today.AddDays(BusinessConstants.PaymentDueInDays),
                InvoicedPeriodFrom = DateTime.Today,
                InvoicedPeriodTo = DateTime.Today,
                IsValid = true,
            };

            var errors = await _invoiceRepository.AddInvoiceTask(invoice);
            if (errors != null)
            {
                _logger.Error($"Received errors when generating invoice for agreement {agreement.Id}: {errors.GetFormattedResponse()}");
            }

            await SendInvoiceEmail(invoice);
        }

        private Task SendInvoiceEmail(Invoice invoice)
        {
            var body = $@"
Jums buvo sugeneruota nauja sąskaita. Sutarties Nr. {invoice.AgreementId}.
Ją galite peržiūrėti {MessageConstants.SiteUrl($"/agreement/{invoice.AgreementId}/invoice/{invoice.Id}")}.";

            return _emailService.SendEmailToAsync(invoice.Agreement.Tenant.Email, MessageConstants.NewInvoiceSubject, body);
        }
    }
}