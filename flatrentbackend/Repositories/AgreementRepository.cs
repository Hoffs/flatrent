using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.BusinessRules;
using FlatRent.BusinessRules.Builder;
using FlatRent.BusinessRules.Builder.Extensions;
using FlatRent.BusinessRules.Inference;
using FlatRent.BusinessRules.Inference.Terms;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Models.Requests;
using FlatRent.Repositories.Abstractions;
using FlatRent.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
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
            agreement.FlatId = flatId;
            agreement.Flat = flat;

            agreement.StatusId = AgreementStatus.Statuses.Requested;
            agreement.Attachments.SetProperty(a => a.AuthorId, userId);
            agreement.Price = (await Context.Flats.FindAsync(flatId)).Price;
            agreement.AuthorId = userId;
            agreement.Author = await Context.Users.FindAsync(userId);
            agreement.Conversation = new Conversation
            {
                AuthorId = userId,
                RecipientId = flat.AuthorId,
                Subject = flat.Name,
            };

//            var rules = RuleBuilder
//                .If<Agreement, RuleResult>(_agreement => _agreement.AuthorId != _agreement.Flat.AuthorId)
//                    .ThenIf(_agreement => _agreement.Flat.ActiveAgreement == null)
//                        .ThenIf(_agreement => _agreement.RentPeriodDays <= BusinessConstants.MaxRentPeriodDays)
//                            .ThenIf(_agreement => _agreement.RentPeriodDays >= _agreement.Flat.MinimumRentDays)
//                                .ReturnThen(true)
//                                .ReturnElse(false, new FormError("To", string.Format(Errors.FlatRentPeriodGreater, BusinessConstants.MinRentPeriodDays)))
//                            .ReturnElse(false, new FormError("To", string.Format(Errors.FlatRentPeriodLess, BusinessConstants.MaxRentPeriodDays)))
//                        .ReturnElse(false, new FormError(Errors.FlatNotAvailableForRent))
//                    .ReturnElse(false, new FormError(Errors.TenantCantBeOwner))
//                .Build();
//
//            var rules1 = RuleBuilder
//                .If<Agreement, RuleResult>(_agreement => _agreement.AuthorId != _agreement.Flat.AuthorId)
//                    .Then(AgreementRequestRules.FlatOneActiveAgreementRule)
//                        .ThenIf(_agreement => _agreement.RentPeriodDays <= BusinessConstants.MaxRentPeriodDays)
//                            .ThenIf(_agreement => _agreement.RentPeriodDays >= _agreement.Flat.MinimumRentDays)
//                                .ReturnThen(true)
//                                .ReturnElse(false, new FormError("To", string.Format(Errors.FlatRentPeriodGreater, BusinessConstants.MinRentPeriodDays)))
//                            .ReturnElse(false, new FormError("To", string.Format(Errors.FlatRentPeriodLess, BusinessConstants.MaxRentPeriodDays)))
//                        .ReturnElse(false, new FormError(Errors.FlatNotAvailableForRent))
//                .Build();

            var rules2 = RuleBuilder
                .IfInferHas<Agreement, RuleResult>(AgreementFacts.TenantIsOwner)
                    .Then(AgreementRequestRules.FlatOneActiveAgreementRule)
                        .ThenIfInferHas(AgreementFacts.ExceedsMaxPeriod)
                            .ThenIf(_agreement => _agreement.RentPeriodDays >= _agreement.Flat.MinimumRentDays)
                                .ReturnThen(true)
                                .ReturnElse(false, new FormError("To", string.Format(Errors.FlatRentPeriodGreater, BusinessConstants.MinRentPeriodDays)))
                            .ReturnElse(false, new FormError("To", string.Format(Errors.FlatRentPeriodLess, BusinessConstants.MaxRentPeriodDays)))
                        .ReturnElse(false, new FormError(Errors.FlatNotAvailableForRent))
                .Build();



            var result = rules2.Execute(agreement);
            if (!result.Passed) return (new[] { result.Error }, null);

            var errors = await AddAsync(agreement, userId);
            return (errors, agreement);
        }

        public Task<IEnumerable<Agreement>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Agreement> GetQueryable()
        {
            return Context.Agreements.AsQueryable();
        }

        public new Task<IEnumerable<FormError>> DeleteAsync(Guid id)
        {
            return base.DeleteAsync(id);
        }

        public Task<Agreement> GetLoadedAsync(Guid id)
        {
            return Context.Agreements
                .Include(a => a.Author)
                .Include(a => a.Flat)
                .ThenInclude(f => f.Author)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public new Task<IEnumerable<FormError>> UpdateAsync(Agreement agreement)
        {
            return base.UpdateAsync(agreement);
        }
    }
}