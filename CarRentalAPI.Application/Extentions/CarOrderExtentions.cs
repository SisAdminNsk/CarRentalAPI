using CarRentalAPI.Application.Filters;
using CarRentalAPI.Application.Paginations;
using CarRentalAPI.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarRentalAPI.Application.Extentions
{
    public static class CarOrderExtentions
    {
        public static IQueryable<CarOrder> Filter(this IQueryable<CarOrder> query, CarOrderFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Price))
            {
                query = query.Where(x => x.Price == decimal.Parse(filter.Price));
            }

            if (!string.IsNullOrEmpty(filter.StartOfLease))
            {
                query = query.Where(x => x.StartOfLease.Date == DateTime.Parse(filter.StartOfLease).Date);
            }

            if (!string.IsNullOrEmpty(filter.EndOfLease))
            {
                query = query.Where(x => x.EndOfLease.Date == DateTime.Parse(filter.EndOfLease).Date);
            }

            return query;
        }

        public static IQueryable<CarOrder> Sort(this IQueryable<CarOrder> query, SortParams sortParams)
        {
            if(sortParams.SortDirection == SortDirection.Descending)
            {
                return query.OrderByDescending(GetKeySelector(sortParams.OrderByField));
            }

            return query.OrderBy(GetKeySelector(sortParams.OrderByField));
        }

        private static Expression<Func<CarOrder, object>> GetKeySelector(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return x => x.EndOfLease;
            }

            return key switch
            {
                nameof(CarOrder.StartOfLease) => x => x.StartOfLease,
                nameof(CarOrder.EndOfLease) => x => x.EndOfLease,
                nameof(CarOrder.Price) => x => x.Price,

                _ => x => x.EndOfLease
            } ;
        }

        public static async Task<PagedResult<CarOrder>> Page(this IQueryable<CarOrder> query, PageParams pageParams)
        {
            var count = await query.CountAsync();

            if(count == 0)
            {
                return new PagedResult<CarOrder>([], 0);
            }

            var page = pageParams.Page ?? 1;
            var pageSize = pageParams.PageSize ?? 5;

            var skip = (page - 1) * pageSize;

            var result = await query.Skip(skip).Take(pageSize).ToArrayAsync();

            return new PagedResult<CarOrder>(result, count);
        }
    }
}
