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

        public async Task<(IEnumerable<FormError>, Guid)> AddAgreementAsync(Guid flatId, Guid userId,
            RentAgreementForm form)
        {
            var agreement = Mapper.Map<Agreement>(form);

            agreement.TenantId = userId;
            agreement.FlatId = flatId;
            agreement.StatusId = AgreementStatus.Statuses.Requested;
            agreement.Attachments.SetProperty(a => a.AuthorId, userId);
            agreement.Price = (await Context.Flats.FindAsync(flatId)).Price;

            var errors = await AddAsync(agreement, userId);
            return (errors, agreement.Id);
        }

        public Task<IEnumerable<FormError>> DeleteAsync(Guid id)
        {
            return base.DeleteAsync(id);
        }

        public Task<IEnumerable<FormError>> UpdateAsync(Agreement agreement)
        {
            return base.UpdateAsync(agreement);
        }
    }
}