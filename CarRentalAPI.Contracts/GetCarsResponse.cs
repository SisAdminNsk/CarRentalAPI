using CarRentalAPI.Core;

namespace CarRentalAPI.Contracts
{
    public class GetCarsResponse
    {
        public int TotalItem { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }   
        public List<Car> Cars { get; set; }

        public GetCarsResponse(int totalItems, int pageNumber, int pageSize, List<Car> cars)
        {
            TotalItem = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Cars = cars;
        }
    }
}
