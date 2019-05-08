using System.Threading.Tasks;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Services.Interfaces;
using Serilog;

namespace FlatRent.Services
{
    public class AgreementService : IAgreementService
    {
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;

        public AgreementService(IEmailService emailService, ILogger logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task SendNewAgreementEmailAsync(Agreement agreement)
        {
            var body = $@"
Buvo sukurta nauja nuomos užklausa. Sutarties Nr. {agreement.Id}.
Ją galite peržiūrėti {MessageConstants.SiteUrl($"/agreement/{agreement.Id}")}.";

            await _emailService.SendEmailToAsync(agreement.Tenant.Email, MessageConstants.NewAgreementSubject, body);
            await _emailService.SendEmailToAsync(agreement.Flat.Author.Email, MessageConstants.NewAgreementSubject, body);
        }
    }
}