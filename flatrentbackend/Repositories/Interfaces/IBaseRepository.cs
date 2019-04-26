using System;
using System.Threading.Tasks;

namespace FlatRent.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity>
    {
        Task<bool> ExistsAsync(Guid id);
        Task<TEntity> GetAsync(Guid id);
    }
}