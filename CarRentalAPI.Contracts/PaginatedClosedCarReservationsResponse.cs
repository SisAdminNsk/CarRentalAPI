namespace CarRentalAPI.Contracts
{
    public class PaginatedClosedCarReservationsResponse
    {
        public int TotalItems { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public List<ClosedCarReservationResponse> Items { get; set; }

        public PaginatedClosedCarReservationsResponse(int totalItems, int page, int pageSize, List<ClosedCarReservationResponse> items)
        {
            TotalItems = totalItems;
            Page = page;
            PageSize = pageSize;
            Items = items;
        }
    }
}
