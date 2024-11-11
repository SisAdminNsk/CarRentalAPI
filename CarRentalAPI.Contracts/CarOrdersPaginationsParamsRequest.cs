using CarRentalAPI.Core.Validation;
using CarRentalAPI.Core;

using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Contracts
{
    public class CarOrdersPaginationsParamsRequest
    {
        [Range(1, 1000)]
        public int PageNumber { get; set; }
        [Range(1, 1000)]
        public int PageSize { get; set; }
        [SortOrderPagination]
        public string SortOrder { get; set; }
        [SortByPagination<CarOrder>]
        public string SortBy { get; set; }

        public CarOrdersPaginationsParamsRequest(int pageNumber, int pageSize, string sortBy, string sortOrder)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SortBy = sortBy;
            SortOrder = sortOrder;
        }

        public CarOrdersPaginationsParamsRequest()
        {

        }
    }
}
