using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlatRent.BusinessRules;
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
                await CheckAgreements();
                await Task.Delay(TimeSpan.FromHours(6), cancellationToken);
            }
        }

        private async Task CheckAgreements()
        {
            await CheckEndedAgreements();
            await CheckExpiredAgreements();
        }

        private async Task CheckEndedAgreements()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                _logger.Information($"Checking ended agreements");
                var repository = scope.ServiceProvider.GetService<IAgreementRepository>();
                var (passed, error) = await AgreementLifetimeRules.DayAfterAgreementEndShouldBeSetToEnded(repository);
                if (!passed)
                {
                    _logger.Error($"Error occured while ending agreements, {error}");
                }
            }
        }

        private async Task CheckExpiredAgreements()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                _logger.Information($"Checking expired agreements");
                var repository = scope.ServiceProvider.GetService<IAgreementRepository>();
                var (passed, error) = await AgreementLifetimeRules.DayAfterAgreementStartShouldBeSetToExpired(repository);
                if (!passed)
                {
                    _logger.Error($"Error occured while expiring agreements, {error}");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}