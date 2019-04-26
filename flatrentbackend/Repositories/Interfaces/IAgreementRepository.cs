using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatRent.Entities;
using FlatRent.Models;
using FlatRent.Models.Requests;

namespace FlatRent.Repositories.Interfaces
{
    public interface IAgreementRepository : IAuthoredBaseRepository<Agreement>
    {
        Task<IEnumerable<FormError>> CancelAgreementAsync(Guid id);
        Task<(IEnumerable<FormError>, Guid)> AddAgreementAsync(Guid id, Guid userId, RentAgreementForm form);
    }
}