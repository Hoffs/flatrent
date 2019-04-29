using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Models.Requests;
using FlatRent.Repositories.Abstractions;
using FlatRent.Repositories.Interfaces;
using Serilog;

namespace FlatRent.Repositories
{
    public class FaultRepository : AuthoredBaseRepository<Fault>, IFaultRepository
    {
        public FaultRepository(DataContext context, IMapper mapper, ILogger logger) : base(context, mapper, logger)
        {
        }

        public Task<IEnumerable<FormError>> CreateFaultAsync(Guid agreementId, FaultForm form)
        {
            var fault = Mapper.Map<Fault>(form);
            fault.AgreementId = agreementId;
            fault.Attachments.SetProperty(a => a.Fault, fault);

            return base.AddAsync(fault);
        }

        public Task<IEnumerable<FormError>> UpdateAsync(Fault entity)
        {
            return base.UpdateAsync(entity);
        }

        public Task<IEnumerable<FormError>> DeleteAsync(Fault entity)
        {
            return base.DeleteAsync(entity);
        }
    }
}