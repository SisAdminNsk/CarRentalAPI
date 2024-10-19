using CarRentalAPI.Core;
using CarRentalAPI.Core.Validation;

namespace CarRentalAPI.Contracts
{
    public class OpenCarReservationRequest
    {
        public Guid CarOrderId { get; set; }

        [LeaseValidation]
        public LeaseDateTime LeaseDateTime { get; set; }
        public decimal Price { get; set; }
        public OpenCarReservationRequest(
            Guid carOrderId,
            LeaseDateTime leaseDateTime,
            decimal price) 
        {
            CarOrderId = carOrderId;
            LeaseDateTime = leaseDateTime;
            Price = price;
        }
    }
}
