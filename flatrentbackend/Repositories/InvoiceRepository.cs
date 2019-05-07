using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Constants;
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

        public IQueryable<Invoice> GetToBeInvoicedListAsync()
        {
            return Context.Invoices.Where(i => !i.Agreement.Deleted
                                        && i.IsValid
                                        && i.InvoicedPeriodTo < DateTime.Today // Valid but less than today
                                        && i.Agreement.To != i.InvoicedPeriodTo // Last invoice
            );
        }

        public async Task<IEnumerable<FormError>> AddAndUpdateTask(Invoice toAdd, Invoice toUpdate)
        {
            using (var transaction = await Context.Database.BeginTransactionAsync())
            {
                try
                {
                    Context.Invoices.Update(toUpdate);
                    await Context.Invoices.AddAsync(toAdd);
                    await Context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Logger.Error(e, "Exception occured while adding and updating invoices");
                    return new[] { new FormError(Errors.Exception) };
                }
            }
            return null;
        }
    }
}