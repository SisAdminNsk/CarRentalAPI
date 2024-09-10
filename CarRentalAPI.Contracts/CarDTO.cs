namespace CarRentalAPI.Contracts
{
    public class CarDTO
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Power { get; set; }
        public string CarClass { get; set; }
        public decimal BaseRentalPricePerHour { get; set; }
        public string CarImageURI { get; set; }
        public CarDTO(

            Guid id,
            string brand,
            string model,
            int power,
            string carClass,
            decimal baseRentalPricePerHour,
            string carImageURI
            )
        {
            Id = id;
            Brand = brand;
            Model = model;
            Power = power;
            CarClass = carClass;
            CarImageURI = carImageURI;
            BaseRentalPricePerHour = baseRentalPricePerHour;
        }
    }
}
