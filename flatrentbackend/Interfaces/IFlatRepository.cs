using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatRent.Entities;
using FlatRent.Models;
using FlatRent.Models.Requests;

namespace FlatRent.Interfaces
{
    public interface IFlatRepository
    {
        Task<IEnumerable<FormError>> AddFlatAsync(FlatForm form);
        Task<IEnumerable<FormError>> DeleteFlatAsync(Guid flatId);
        Task<Flat> GetFlatAsync(Guid flatId);
        Task<IEnumerable<Flat>> GetFlatsAsync(bool includeRented = false, int count = 20, int offset = 0);
        Task<int> GetFlatCountAsync(bool includeRented = false);
        Task<IEnumerable<FormError>> AddAgreemenTask(Guid flatId, Guid userId, RentAgreementForm form);
    }
}