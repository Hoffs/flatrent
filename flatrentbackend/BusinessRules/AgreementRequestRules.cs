using System;
using FlatRent.BusinessRules.Builder;
using FlatRent.BusinessRules.Builder.Extensions;
using FlatRent.BusinessRules.Builder.Interfaces;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Models;
using FlatRent.Models.Requests;

namespace FlatRent.BusinessRules
{
    public static class AgreementRequestRules
    {
        /// Business Rule: DP.1.3.
        public static (bool, FormError) PeriodMustBeLongerOrEqualToSpecified(Flat flat, AgreementForm form)
        {
            var rentPeriod = TimeSpan.FromTicks(form.To.Date.Ticks - form.From.Date.Ticks).Days;
            if (rentPeriod >= flat.MinimumRentDays)
            {
                return (true, null);
            }

            return (false,
                new FormError("To", string.Format(Errors.FlatRentPeriodGreater, BusinessConstants.MinRentPeriodDays)));
        }

        /// Business Rule: DP.1.4.
        public static (bool, FormError) PeriodMustBeShorterOrEqualToMaximum(Flat flat, AgreementForm form)
        {
            var rentPeriod = TimeSpan.FromTicks(form.To.Date.Ticks - form.From.Date.Ticks).Days;
            if (rentPeriod <= BusinessConstants.MaxRentPeriodDays)
            {
                return (true, null);
            }

            return (false,
                new FormError("To", string.Format(Errors.FlatRentPeriodLess, BusinessConstants.MaxRentPeriodDays)));
        }

        /// Business Rule: DP.1.2.
        public static (bool, FormError) FlatMustHaveAtMostOneActiveAgreement(Flat flat)
        {
            if (flat.ActiveAgreement == null)
            {
                return (true, null);
            }

            return (false, new FormError(Errors.FlatNotAvailableForRent));
        }

        public static IRule<Agreement, RuleResult> FlatOneActiveAgreementRule =
            RuleBuilder
                .If<Agreement, RuleResult>(agreement => agreement.Flat.ActiveAgreement != null)
                    .ReturnThen(true)
                    .ReturnElse(false, new FormError(Errors.FlatNotAvailableForRent));

        /// Business Rule: DP.1.1.
        public static (bool, FormError) TenantCantBeOwner(Flat flat, Guid tenantId)
        {
            if (tenantId != flat.AuthorId)
            {
                return (true, null);
            }

            return (false, new FormError(Errors.TenantCantBeOwner));
        }

        public static IRule<Agreement, RuleResult> TenantCantBeOwnerRule =
            RuleBuilder
                .If<Agreement, RuleResult>(agreement => agreement.AuthorId != agreement.Flat.AuthorId)
                    .ReturnThen(true)
                    .ReturnElse(false, new FormError(Errors.TenantCantBeOwner));


    }
}