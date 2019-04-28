using System.Collections.Generic;
using System.Threading.Tasks;
using FlatRent.Entities;
using FlatRent.Models;

namespace FlatRent.Repositories.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<IEnumerable<FormError>> AddInvoiceTask(Invoice invoice);
        Task<IEnumerable<FormError>> UpdateInvoiceTask(Invoice invoice);
    }
}