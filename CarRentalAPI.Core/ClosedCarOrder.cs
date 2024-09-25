using CSharpFunctionalExtensions;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Core
{
    public class ClosedCarOrder : Entity<Guid>
    {
        public CarOrder CarOrder { get; set; }

        [Required]
        [MaxLength(100)]
        public string Status { get; set; }

        public ClosedCarOrder()
        {
            Id = Guid.NewGuid();
        }

        public Guid CarId { get; set; }

        public ClosedCarOrder(CarOrder carOrder, string status)
        {
            CarOrder = carOrder;
            Status = status;
            CarId = carOrder.CarId;
        }
    }
}
