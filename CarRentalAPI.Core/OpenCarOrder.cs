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
            CarId = carOrder.CarId; 
        }
        public Car Car { get; set; }
        public Guid CarId { get; set; }
    }
}
