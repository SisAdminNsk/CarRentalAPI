using CSharpFunctionalExtensions;
using System.Net.Sockets;

namespace CarRentalAPI.Core
{
    public class Car : Entity<Guid>
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Power { get; set; }
        public string CarClass { get; set; }
        public decimal BaseRentalPricePerHour { get; set; }
        public byte[] ImageData { get; set; }

        public Car()
        {
            Id = Guid.NewGuid();
        }

        public Car(
            Guid id,
            string brand,
            string model,
            string carClass,
            byte[] imageData,
            decimal baseRentalPricePerHour)
        {
            Id = id;
            Brand = brand;
            Model = model;
            CarClass = carClass;
            ImageData = imageData;
            BaseRentalPricePerHour = baseRentalPricePerHour;
        }
    }
}
