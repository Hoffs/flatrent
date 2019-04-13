using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatRent.Models;

namespace FlatRent.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity>
    {
        Task<bool> ExistsAsync(Guid id);
        Task<bool> IsAuthorAsync(Guid id, Guid createdBy);
        Task<TEntity> GetAsync(Guid id);
    }
}