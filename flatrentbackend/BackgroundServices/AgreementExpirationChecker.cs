using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlatRent.Entities;
using FlatRent.Repositories.Interfaces;
using FlatRent.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace FlatRent.BackgroundServices
{
    public class AgreementExpirationChecker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public AgreementExpirationChecker(IServiceProvider serviceProvider, ILogger logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await CheckEndedAgreements();
                await CheckExpiredAgreements();
                await Task.Delay(TimeSpan.FromHours(6), cancellationToken);
            }
        }

        private async Task CheckEndedAgreements()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                _logger.Information($"Checking ended agreements");
                var repository = scope.ServiceProvider.GetService<IAgreementRepository>();
                var queryable = repository.GetQueryable();
                var ended = queryable.Where(a =>
                    a.To < DateTime.Today && a.StatusId == AgreementStatus.Statuses.Accepted && !a.Deleted).ToList();
                foreach (var agreement in ended)
                {
                    agreement.StatusId = AgreementStatus.Statuses.Ended;
                    await repository.UpdateAsync(agreement);
                    _logger.Information($"Ended agreement {agreement.Id}");
                }
            }
        }

        private async Task CheckExpiredAgreements()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                _logger.Information($"Checking expired agreements");
                var repository = scope.ServiceProvider.GetService<IAgreementRepository>();
                var queryable = repository.GetQueryable();
                var expired = queryable.Where(a =>
                    a.From < DateTime.Today && a.StatusId == AgreementStatus.Statuses.Requested && !a.Deleted).ToList();
                foreach (var agreement in expired)
                {
                    agreement.StatusId = AgreementStatus.Statuses.Expired;
                    await repository.UpdateAsync(agreement);
                    _logger.Information($"Expired agreement {agreement.Id}");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}