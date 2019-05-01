using System.Linq;
using FlatRent.Constants;

namespace FlatRent.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, int skip = 0,
            int take = PaginationConstants.StandardPageSize)
        {
            return queryable.Skip(skip).Take(take);
        }
    }
}