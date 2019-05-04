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

        public async Task<(IEnumerable<FormError>, Fault)> CreateFaultAsync(Guid agreementId, FaultForm form, Guid userId)
        {
            var agreement = await Context.Agreements.FindAsync(agreementId);
            var fault = Mapper.Map<Fault>(form);
            fault.AuthorId = userId;
            fault.AgreementId = agreementId;
            fault.Attachments.SetProperty(a => a.Fault, fault);
            fault.Attachments.SetProperty(a => a.AuthorId, userId);
            fault.Conversation = new Conversation
            {
                AuthorId = userId,
                RecipientId = agreement.Flat.AuthorId,
                Subject = fault.Name,
            };

            return (await base.AddAsync(fault), fault);
        }

        public new Task<IEnumerable<FormError>> UpdateAsync(Fault entity)
        {
            return base.UpdateAsync(entity);
        }

        public new Task<IEnumerable<FormError>> DeleteAsync(Fault entity)
        {
            return base.DeleteAsync(entity);
        }
    }
}