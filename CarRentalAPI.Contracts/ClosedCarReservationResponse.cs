namespace CarRentalAPI.Contracts
{
    public class ClosedCarReservationResponse
    {
        public Guid Id { get; set; }
        public Guid CarId { get; set; }

        public Guid CarharingUserId { get; set;  }
        public DateTime StartOfLease { get; set; }
        public DateTime EndOfLease { get; set; }
        public string Status { get; set; }

        public ClosedCarReservationResponse()
        {

        }

        public ClosedCarReservationResponse(

            Guid id,
            Guid carId,
            Guid carharingUserId,
            DateTime startOfLease,
            DateTime endOfLease,
            string status)
        {
            Id = id;
            CarId = carId;
            CarharingUserId = carharingUserId;
            StartOfLease = startOfLease;
            EndOfLease = endOfLease;
            Status = status;
        }
    }
}
