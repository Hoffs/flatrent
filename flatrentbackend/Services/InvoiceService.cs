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
                DueDate = agreement.From,
            };

            var errors = await _invoiceRepository.AddInvoiceTask(invoice);
            if (errors != null)
            {
                _logger.Error($"Received errors when generating invoice for agreement {agreementId}: {errors.GetFormattedResponse()}");
            }
        }

        public async Task GenerateInvoiceForAgreementAsync(Guid agreementId)
        {
            var agreement = await _agreementRepository.GetAsync(agreementId);
            if (agreement == null || agreement.Deleted || agreement.StatusId != AgreementStatus.Statuses.Accepted)
            {
                throw new ArgumentException("Agreement doesn't exist or is not accepted.", nameof(agreementId));
            }

            var lastInvoice = agreement.Invoices.OrderByDescending(i => i.CreatedDate).First();

            if (lastInvoice.DueDate.Date == agreement.To.Date)
            {
                return;
            }

            var dueDate = lastInvoice.DueDate.AddDays(BusinessConstants.RentMonthInDays);
            var amountToPay = agreement.Price;
            if (dueDate > agreement.To)
            {
                dueDate = agreement.To;
                var daysLeft = agreement.To.Subtract(lastInvoice.DueDate).Days;
                amountToPay = (float) Math.Round(amountToPay * (BusinessConstants.RentMonthInDays / daysLeft), 2, MidpointRounding.AwayFromZero);
            }

            var notInvoicedFaults = agreement.Faults.Where(Fault.NotInvoicedFaultsFunc).ToList();

            var faultPrice = notInvoicedFaults.Sum(f => f.Price);

            var invoice = new Invoice
            {
                AgreementId = agreementId,
                AmountToPay = faultPrice + amountToPay,
                Faults = notInvoicedFaults,
                DueDate = dueDate,
            };

            var errors = await _invoiceRepository.AddInvoiceTask(invoice);
            if (errors != null)
            {
                _logger.Error($"Received errors when generating invoice for agreement {agreementId}: {errors.GetFormattedResponse()}");
            }
        }
    }
}