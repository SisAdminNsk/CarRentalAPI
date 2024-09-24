using CSharpFunctionalExtensions;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Core
{
    public class CarOrder : Entity<Guid>
    {
        public DateTime StartOfLease { get; set; }
        public DateTime EndOfLease { get; set; }
        public CarsharingUser Customer { get; set; }
        public Car Car { get; set; }
        public Guid CarId { get; set; }
        [MaxLength(100)]
        public string Comment { get; set; }
        public decimal Price { get; set; }
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
            decimal price)
        {
            Comment = comment;
            StartOfLease = startOfLease;
            EndOfLease = endOfLease;
            Customer = customer;
            Car = car;

            CarId = car?.Id ?? Guid.Empty;
            Price = price;
        }
    }
}
