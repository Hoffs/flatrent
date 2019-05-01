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
    public class AgreementRepository : AuthoredBaseRepository<Agreement>, IAgreementRepository
    {

        public AgreementRepository(DataContext context, IMapper mapper, ILogger logger) : base(context, mapper, logger)
        {
        }

        public async Task<(IEnumerable<FormError>, Agreement)> AddAgreementAsync(Guid flatId, Guid userId,
            AgreementForm form)
        {
            var agreement = Mapper.Map<Agreement>(form);
            var flat = await Context.Flats.FindAsync(flatId);
            agreement.TenantId = userId;
            agreement.FlatId = flatId;
            agreement.StatusId = AgreementStatus.Statuses.Requested;
            agreement.Attachments.SetProperty(a => a.AuthorId, userId);
            agreement.Price = (await Context.Flats.FindAsync(flatId)).Price;
            agreement.Conversation = new Conversation
            {
                AuthorId = userId,
                RecipientId = flat.AuthorId,
                Subject = flat.Name,
            };

            var errors = await AddAsync(agreement, userId);
            return (errors, agreement);
        }

        public new Task<IEnumerable<FormError>> DeleteAsync(Guid id)
        {
            return base.DeleteAsync(id);
        }

        public new Task<IEnumerable<FormError>> UpdateAsync(Agreement agreement)
        {
            return base.UpdateAsync(agreement);
        }
    }
}