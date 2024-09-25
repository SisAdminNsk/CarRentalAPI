namespace CarRentalAPI.Contracts
{
    public class OpenCarReservationRequest
    {
        public Guid CarOrderId { get; set; }
        public DateTime StartOfLease { get; set; }
        public DateTime EndOfLease { get; set; }
        public decimal Price { get; set; }
        public OpenCarReservationRequest(
            Guid carOrderId,
            DateTime startOfLease,
            DateTime endOfLease,
            decimal price) 
        {
            CarOrderId = carOrderId;
            StartOfLease = startOfLease;
            EndOfLease = endOfLease;
            Price = price;
        }
    }
}
