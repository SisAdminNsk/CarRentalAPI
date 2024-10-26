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
        public string Comment { get; set; }
        public string CarImageURI { get; set; }
        public string CarName { get; set; }
        public double Price { get; set; }

        public ClosedCarReservationResponse()
        {

        }

        public ClosedCarReservationResponse(

            Guid id,
            Guid carId,
            Guid carharingUserId,
            DateTime startOfLease,
            DateTime endOfLease,
            string status,
            string comment,
            string carImageUri,
            double price,
            string carName)
        {
            Id = id;
            CarId = carId;
            CarharingUserId = carharingUserId;
            StartOfLease = startOfLease;
            EndOfLease = endOfLease;
            Status = status;
            Comment = comment;
            CarImageURI = carImageUri;
            Price = price;
            CarName = carName;
        }
    }
}
