using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FlatRent.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;
using Serilog;

namespace FlatRent.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly SmtpClient _smtpClient;
        private readonly RestClient _rest;

        public EmailService(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
            _smtpClient = new SmtpClient(_configuration["SMTP:Hostname"])
            {
                Credentials =
                    new NetworkCredential(_configuration["SMTP:Username"], _configuration["SMTP:Password"])
            };

            _rest = new RestClient($"{_configuration["Mailgun:APIUrl"]}/messages")
            {
                Authenticator = new HttpBasicAuthenticator("api", _configuration["Mailgun:APIKey"])
            };
        }

//        public Task SendEmailToAsync(string to, string subject, string body)
//        {
//            var message = new MailMessage { From = new MailAddress(_configuration["SMTP:From"]) };
//            //            message.To.Add(to);
//            
//            // SANDBOX ONLY ALLOWS AUTHORIZED RECIPIENTS
//
//            message.To.Add("ignasmaslinskas@gmail.com");
//            message.Subject = subject;
//            message.Body = body;
//
//            return _smtpClient.SendMailAsync(message);
//        }
        public async Task SendEmailToAsync(string to, string subject, string body)
        {
            // SANDBOX ONLY ALLOWS AUTHORIZED RECIPIENTS
            var request = new RestRequest();
            request.AddParameter("from", _configuration["SMTP:From"]);
            request.AddParameter("to", "ignasmaslinskas@gmail.com");
            request.AddParameter("subject", subject);
            request.AddParameter("text", body);
            var response = await _rest.ExecutePostTaskAsync(request);
        }
    }
}