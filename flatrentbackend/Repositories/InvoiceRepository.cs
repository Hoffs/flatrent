using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Entities;
using FlatRent.Models;
using FlatRent.Repositories.Abstractions;
using FlatRent.Repositories.Interfaces;
using Serilog;

namespace FlatRent.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(DataContext context, IMapper mapper, ILogger logger) : base(context, mapper, logger)
        {
        }

        public Task<IEnumerable<FormError>> AddInvoiceTask(Invoice invoice)
        {
            return base.AddAsync(invoice);
        }

        public Task<IEnumerable<FormError>> UpdateInvoiceTask(Invoice invoice)
        {
            return base.UpdateAsync(invoice);
        }
    }
}