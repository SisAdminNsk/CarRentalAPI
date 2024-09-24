namespace CarRentalAPI.Contracts
{
    public class CreateCarOrderRequest
    {
        public Guid CarId { get; set; }
        public Guid CarsharingUserId { get; set; }
        public DateTime StartOfLease { get; set; }
        public DateTime EndOfLease { get; set; }

        public CreateCarOrderRequest(Guid carId, Guid carsharingUserId, DateTime startOfLease, DateTime endOfLease) 
        {
            CarId = carId;
            CarsharingUserId = carsharingUserId;
            StartOfLease = startOfLease;
            EndOfLease = endOfLease;
        }
    }
}
