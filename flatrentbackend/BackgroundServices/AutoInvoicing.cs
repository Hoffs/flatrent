using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlatRent.Repositories.Interfaces;
using FlatRent.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace FlatRent.BackgroundServices
{
    public class AutoInvoicing : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public AutoInvoicing(IServiceProvider serviceProvider, ILogger logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await InvoiceAgreements();
                await Task.Delay(TimeSpan.FromHours(6), cancellationToken);
            }
        }

        private async Task InvoiceAgreements()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                _logger.Information($"Trying to generate invoices");
                var repository = scope.ServiceProvider.GetService<IInvoiceRepository>();
                var invoiceService = scope.ServiceProvider.GetService<IInvoiceService>();
                var list = repository.GetToBeInvoicedListAsync();
                foreach (var invoice in list)
                {
                    await invoiceService.GenerateInvoiceForAgreementAsync(invoice.AgreementId);
                    _logger.Information($"Generated invoice for agreement {invoice.AgreementId}");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}