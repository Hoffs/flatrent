using System.Threading.Tasks;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Services.Interfaces;
using Serilog;

namespace FlatRent.Services
{
    public class IncidentService : IIncidentService
    {
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;

        public IncidentService(IEmailService emailService, ILogger logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public Task SendIncidentEmail(Incident incident)
        {
            var body = $@"
Buvo sukurtas naujas incidentas. Sutarties Nr. {incident.AgreementId}.
Jį galite peržiūrėti {MessageConstants.SiteUrl($"/agreement/{incident.AgreementId}/incident/{incident.Id}")}.";

            return _emailService.SendEmailToAsync(incident.Agreement.Flat.Author.Email, MessageConstants.NewAgreementSubject, body);
        }
    }
}