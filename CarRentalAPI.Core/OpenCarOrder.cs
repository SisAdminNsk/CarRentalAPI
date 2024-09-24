using CSharpFunctionalExtensions;

namespace CarRentalAPI.Core
{
    public class OpenCarOrder : Entity<Guid>
    {
        public OpenCarOrder()
        {
            Id = Guid.NewGuid();
        }
        public CarOrder CarOrder { get; set; }
        public OpenCarOrder(CarOrder carOrder)
        {
            CarOrder = carOrder;
        }
    }
}
