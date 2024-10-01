using CarRentalAPI.Core.Validation;
using CSharpFunctionalExtensions;

namespace CarRentalAPI.Core
{
    public class Car : Entity<Guid>
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        [Sortable]
        public int Power { get; set; }
        [Sortable]
        public string CarClass { get; set; }
        [Sortable]
        public decimal BaseRentalPricePerHour { get; set; }
        [Sortable]
        public decimal AccelerationTo100 { get; set; }
        public string CarImageURI { get; set; }
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
            decimal baseRentalPricePerHour,
            decimal accelerationTo100)
        {
            Id = id;
            Brand = brand;
            Model = model;
            CarClass = carClass;
            CarImageURI = carImageURI;
            BaseRentalPricePerHour = baseRentalPricePerHour;
            AccelerationTo100 = accelerationTo100;
        }
    }
}
