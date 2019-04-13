using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatRent.Entities;
using FlatRent.Models;
using FlatRent.Models.Requests;

namespace FlatRent.Repositories.Interfaces
{
    public interface IAgreementRepository : IBaseRepository<Agreement>
    {
        Task<IEnumerable<FormError>> CancelAgreement(Guid id);
        Task<IEnumerable<FormError>> CreateAgreementTask(Guid id, Guid userId, RentAgreementForm form);
    }
}