using System;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Models;

namespace FlatRent.BusinessRules
{
    public class IncidentRules
    {
        public static (bool, FormError) AgreementMustBeActive(Agreement agreement)
        {
            if (agreement.Status.Id == AgreementStatus.Statuses.Accepted)
            {
                return (true, null);
            }

            return (false, new FormError());
        }
    }
}