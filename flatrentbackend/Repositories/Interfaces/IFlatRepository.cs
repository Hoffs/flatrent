﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatRent.Entities;
using FlatRent.Models;
using FlatRent.Models.Requests;
using FlatRent.Models.Requests.Flat;

namespace FlatRent.Repositories.Interfaces
{
    public interface IFlatRepository : IAuthoredBaseRepository<Flat>
    {
        Task<(IEnumerable<FormError>, Guid)> AddFlatAsync(FlatForm form, Guid userId);
        Task<IEnumerable<FormError>> DeleteAsync(Guid flatId);
        Task<IEnumerable<FormError>> UpdateAsync(Flat flat);
        IQueryable<Flat> GetListAsync(bool includeRented = false, int count = 20, int offset = 0);
        Task<int> GetCountAsync(bool includeRented = false);
        Task<(IEnumerable<FormError>, IEnumerable<Image>)> UpdateAsync(Guid flatId, FlatUpdateForm form);
        IQueryable<Flat> GetListAsync(int count, int offset, FlatListFilters filters);
    }
}