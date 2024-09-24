using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Contracts
{
    public class CarOrderRequest
    {
        public Guid CarId { get; set; }
        public Guid CarsharingUserId { get; set; }
        public DateTime StartOfLease { get; set; }
        public DateTime EndOfLease { get; set; }
        [Required]
        [MaxLength(100)]
        public string Comment { get; set; }
        public decimal ApproximatePrice { get; set; }

        public CarOrderRequest(
            Guid carId,
            Guid carsharingUserId,
            DateTime startOfLease,
            DateTime endOfLease, 
            string comment,
            decimal approximatePrice) 
        {
            CarId = carId;
            CarsharingUserId = carsharingUserId;
            StartOfLease = startOfLease;
            EndOfLease = endOfLease;

            Comment = comment;
            ApproximatePrice = approximatePrice;
        }
    }
}
