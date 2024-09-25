namespace CarRentalAPI.Contracts
{
    public class OpenedCarReservationResponse
    {
        public Guid Id { get; set; }
        public Guid CarId { get; set; }
        public string CarName { get; set; }
        public string CarImageUri { get; set; }
        public int RentalTimeRemainInHours { get; set; }


        public decimal Price { get; set; }
        public OpenedCarReservationResponse(
            Guid id,
            Guid carId, 
            string carName,
            string carImageUri,
            int rentalTimeRemainInHours, 
            DateOnly deadlineDate,
            TimeOnly deadLineTime)
        {
            Id = id;
            CarId = carId;
            CarName = carName;
            CarImageUri = carImageUri;
            RentalTimeRemainInHours = rentalTimeRemainInHours;
        }
    }
}
