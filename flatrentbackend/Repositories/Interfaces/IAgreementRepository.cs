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
        Task<Agreement> GetLoadedAsync(Guid id);
        Task<IEnumerable<FormError>> UpdateAsync(Agreement agreement);
        Task<IEnumerable<FormError>> DeleteAsync(Guid id);
        Task<(IEnumerable<FormError>, Agreement)> AddAgreementAsync(Guid id, Guid userId, AgreementForm form);
        Task<IEnumerable<Agreement>> GetListAsync();
    }
}