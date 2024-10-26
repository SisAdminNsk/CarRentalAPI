namespace CarRentalAPI.Contracts
{
    public class OpenedCarReservationResponse
    {
        public Guid Id { get; set; }
        public Guid CarId { get; set; }
        public string CarName { get; set; }
        public string CarImageUri { get; set; }
        public int RentalTimeRemainInSeconds { get; set; }
        public decimal Price { get; set; }
        public DateTime DeadlineDateTime { get; set; }
        public DateTime StartOfLease { get; set; }

        public string Status { get; set; }
        public string Comment { get; set; }

        public OpenedCarReservationResponse(
            Guid id,
            Guid carId, 
            string carName,
            string carImageUri,
            int rentalTimeRemainInSeconds,
            DateTime deadlineDateTime,
            DateTime startOfLease,
            string status,
            string comment)
        {
            Id = id;
            CarId = carId;
            CarName = carName;
            CarImageUri = carImageUri;
            RentalTimeRemainInSeconds = rentalTimeRemainInSeconds;
            DeadlineDateTime = deadlineDateTime;
            StartOfLease = startOfLease;
            Status = status;
            Comment = comment;
        }

        public OpenedCarReservationResponse()
        {

        }
    }
}
