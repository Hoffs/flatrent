using System.Collections.Generic;
using System.Linq;
using FlatRent.Constants;

namespace FlatRent.Extensions
{
    public static class PaginationExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, int skip = 0,
            int take = PaginationConstants.StandardPageSize)
        {
            return queryable.Skip(skip).Take(take);
        }
        public static IEnumerable<T> Paginate<T>(this ICollection<T> collection, int skip = 0,
            int take = PaginationConstants.StandardPageSize)
        {
            return collection.Skip(skip).Take(take);
        }
    }
}