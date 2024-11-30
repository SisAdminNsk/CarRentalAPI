using CSharpFunctionalExtensions;

namespace CarRentalAPI.Application.Paginations
{
    public class PagedResult<T> where T : Entity<Guid>
    {
        public T[] Data { get; set; }

        public int Total { get; set; }

        public PagedResult(T[] data, int total) 
        {
            Data = data;
            Total = total;
        }
    }
}
