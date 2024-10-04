using CarRentalAPI.Core;
using CarRentalAPI.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Contracts
{
    public class CarOrderRequest
    {
        [Required]
        public Guid CarId { get; set; }
        [Required]
        public Guid CarsharingUserId { get; set; }

        [LeaseValidation]
        public LeaseDateTime LeaseDateTime { get; set; }

        [Required]
        [MaxLength(100)]
        public string Comment { get; set; }
        [Required]
        public decimal ApproximatePrice { get; set; }

        public CarOrderRequest(
            Guid carId,
            Guid carsharingUserId,
            LeaseDateTime leaseDateTime,
            string comment,
            decimal approximatePrice) 
        {
            CarId = carId;
            CarsharingUserId = carsharingUserId;

            LeaseDateTime = leaseDateTime;

            Comment = comment;
            ApproximatePrice = approximatePrice;
        }
    }
}
