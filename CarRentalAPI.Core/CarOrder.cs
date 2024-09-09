using CSharpFunctionalExtensions;

namespace CarRentalAPI.Core
{
    public class CarOrder : Entity<Guid>
    {
        public DateTime StartOfLease { get; set; }
        public DateTime EndOfLease { get; set; }
        public CarsharingUser Customer { get; set; }
        public Car Car { get; set; }

        public CarOrder()
        {
            Id = Guid.NewGuid();
        }

        public CarOrder(
            DateTime startOfLease,
            DateTime endOfLease,
            CarsharingUser customer,
            Car car)
        {
            StartOfLease = startOfLease;
            EndOfLease = endOfLease;
            Customer = customer;
            Car = car;
        }
    }
}
