using System.Threading.Tasks;

namespace FlatRent.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailToAsync(string to, string subject, string body);
    }
}