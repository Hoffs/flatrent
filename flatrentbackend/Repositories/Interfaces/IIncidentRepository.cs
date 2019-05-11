using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatRent.Entities;
using FlatRent.Models;
using FlatRent.Models.Requests;

namespace FlatRent.Repositories.Interfaces
{
    public interface IIncidentRepository
    {
        Task<(IEnumerable<FormError>, Incident)> CreateAsync(Guid agreementId, IncidentForm form, Guid userId);
        Task<IEnumerable<FormError>> UpdateAsync(Incident entity);
        Task<IEnumerable<FormError>> DeleteAsync(Incident entity);
        Task<Incident> GetLoadedAsync(Guid incidentId);
    }
}