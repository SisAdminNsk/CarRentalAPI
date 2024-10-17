using CarRentalAPI.Core.Validation;
using CSharpFunctionalExtensions;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Core
{
    public class CarOrder : Entity<Guid>
    {
        public DateTime StartOfLease { get; set; }
        public DateTime EndOfLease { get; set; }
        public CarsharingUser CarsharingUser { get; set; }
        public Guid CarsharingUserId { get; set; }
        public Car Car { get; set; }
        public Guid CarId { get; set; }
        [MaxLength(350)]
        public string Comment { get; set; }
        public decimal Price { get; set; }
        [Required]
        [CarOrderStatusValidation]
        public string Status { get; set; }
        public CarOrder()
        {
            Id = Guid.NewGuid();
        }

        public CarOrder(
            DateTime startOfLease,
            DateTime endOfLease,
            CarsharingUser customer,
            Car car,
            string comment,
            decimal price, 
            string status)
        {
            Id = Guid.NewGuid();

            Comment = comment;
            StartOfLease = startOfLease;
            EndOfLease = endOfLease;

            CarsharingUser = customer;
            CarsharingUserId = customer?.Id ?? Guid.Empty;

            Car = car;
            CarId = car?.Id ?? Guid.Empty;

            Price = price;

            Status = status;
        }
    }
}
