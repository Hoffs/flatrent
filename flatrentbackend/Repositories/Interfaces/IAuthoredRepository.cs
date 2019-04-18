using System;
using System.Threading.Tasks;

namespace FlatRent.Repositories.Interfaces
{
    public interface IAuthoredRepository<TEntity>
    {
        Task<bool> IsAuthorAsync(Guid id, Guid createdBy);
    }
}