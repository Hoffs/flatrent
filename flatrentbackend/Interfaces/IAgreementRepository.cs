using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatRent.Entities;
using FlatRent.Models;

namespace FlatRent.Interfaces
{
    public interface IAgreementRepository
    {
        Task<IEnumerable<FormError>> CancelAgreement(Guid id);
        Task<RentAgreement> GetAgreement(Guid id);
    }
}