using CSharpFunctionalExtensions;

namespace CarRentalAPI.Core
{
    public class Car : Entity<Guid>
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Power { get; set; }
        public string CarClass { get; set; }
        public decimal BaseRentalPricePerHour { get; set; }
        public string CarImageURI { get; set; }
        public List<CarOrder> CarOrders { get; set; }
        public OpenCarOrder OpenCarOrder { get; set; }

        public List<ClosedCarOrder> ClosedCarOrders { get; set; }
        public Car()
        {
            Id = Guid.NewGuid();
        }

        public Car(
            Guid id,
            string brand,
            string model,
            string carClass,
            string carImageURI,
            decimal baseRentalPricePerHour)
        {
            Id = id;
            Brand = brand;
            Model = model;
            CarClass = carClass;
            CarImageURI = carImageURI;
            BaseRentalPricePerHour = baseRentalPricePerHour;
        }
    }
}
