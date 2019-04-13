using FlatRent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FlatRent.Extensions
{
    public static class DbSetExtensions
    {
        public static EntityEntry<T> Delete<T>(this DbSet<T> dbSet, T entity) where T : BaseEntity
        {
            entity.Deleted = true;
            return dbSet.Update(entity);
        }
    }
}