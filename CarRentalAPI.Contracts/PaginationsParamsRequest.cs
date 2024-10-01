using CarRentalAPI.Core;
using CarRentalAPI.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Contracts
{
    public class PaginationsParamsRequest
    {
        [Range(1, 1000)]
        public int PageNumber { get; set; }
        [Range(1, 1000)]
        public int PageSize { get; set; }
        [SortOrderPagination]
        public string SortOrder { get; set; }
        [SortByPagination<Car>]
        public string SortBy { get; set; }

        public PaginationsParamsRequest(int pageNumber, int pageSize, string sortBy, string sortOrder)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SortBy = sortBy;
            SortOrder = sortOrder;
        }

        public PaginationsParamsRequest()
        {

        }
    }
}
