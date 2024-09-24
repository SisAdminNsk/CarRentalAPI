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

        public ClosedCarOrder(CarOrder carOrder, string status)
        {
            CarOrder = carOrder;
            Status = status;
        }
    }
}
