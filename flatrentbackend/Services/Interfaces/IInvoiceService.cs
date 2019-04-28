using System;
using System.Threading.Tasks;

namespace FlatRent.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task GenerateInitialInvoiceAsync(Guid agreementId);
        Task GenerateInvoiceForAgreementAsync(Guid agreementId);
    }
}