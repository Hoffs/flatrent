using System.Threading.Tasks;
using FlatRent.Entities;

namespace FlatRent.Services.Interfaces
{
    public interface IAgreementService
    {
        Task SendNewAgreementEmailAsync(Agreement agreement);
    }
}