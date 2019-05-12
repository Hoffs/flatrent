using System;
using System.Linq;
using System.Threading.Tasks;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Models;
using FlatRent.Repositories.Interfaces;

namespace FlatRent.BusinessRules
{
    public static class AgreementLifetimeRules
    {
        public static (bool, FormError) CanRejectOnlyWhenRequested(Agreement agreement)
        {
            if (agreement.StatusId == AgreementStatus.Statuses.Requested)
            {
                return (true, null);
            }

            return (false, new FormError());
        }

        public static (bool, FormError) CanDeleteOnlyWhenRequested(Agreement agreement)
        {
            if (agreement.StatusId == AgreementStatus.Statuses.Requested)
            {
                return (true, null);
            }

            return (false, new FormError());
        }

        public static (bool, FormError) CanAcceptOnlyWhenRequested(Agreement agreement)
        {
            if (agreement.StatusId == AgreementStatus.Statuses.Requested)
            {
                return (true, null);
            }

            return (false, new FormError());
        }

        public static async Task<(bool, FormError)> DayAfterAgreementEndShouldBeSetToEnded(IAgreementRepository repository)
        {
            var queryable = repository.GetQueryable();
            var ended = queryable.Where(a =>
                a.To < DateTime.Today && a.StatusId == AgreementStatus.Statuses.Accepted && !a.Deleted).ToList();

            foreach (var agreement in ended)
            {
                agreement.StatusId = AgreementStatus.Statuses.Ended;
                var result = await repository.UpdateAsync(agreement);
                if (result != null) return (false, result.First());
            }

            return (true, null);
        }

        public static async Task<(bool, FormError)> DayAfterAgreementStartShouldBeSetToExpired(IAgreementRepository repository)
        {
            var queryable = repository.GetQueryable();
            var expired = queryable.Where(a =>
                a.From < DateTime.Today && a.StatusId == AgreementStatus.Statuses.Requested && !a.Deleted).ToList();

            foreach (var agreement in expired)
            {
                agreement.StatusId = AgreementStatus.Statuses.Expired;
                await repository.UpdateAsync(agreement);
            }

            return (true, null);
        }
    }
}