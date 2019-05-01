using System;
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
        private readonly ILogger _logger;

        public InvoiceService(IAgreementRepository agreementRepository, IInvoiceRepository invoiceRepository, ILogger logger)
        {
            _agreementRepository = agreementRepository;
            _invoiceRepository = invoiceRepository;
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
            if (lastInvoice.InvoicedPeriodTo.Date == agreement.To.Date)
            {
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

                var daysLeft = agreement.To.Subtract(invoicedPeriodStart.Date).Days;
                amountToPay = (float) Math.Round(agreement.Price * (BusinessConstants.RentMonthInDays / daysLeft), 2, MidpointRounding.AwayFromZero);
            }

            // Calculate and add faults to invoice
            var notInvoicedFaults = agreement.Faults.Where(Fault.NotInvoicedFaultsFunc).ToList();
            var faultPrice = notInvoicedFaults.Sum(f => f.Price);

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
                Faults = notInvoicedFaults,
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
        }
    }
}