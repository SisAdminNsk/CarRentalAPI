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
        public byte[] ImageData {  get; set; }
        public CarDTO(

            Guid id,
            string brand,
            string model,
            int power,
            string carClass,
            decimal baseRentalPricePerHour,
            byte[] imageData)
        {
            Id = id;
            Brand = brand;
            Model = model;
            Power = power;
            CarClass = carClass;
            BaseRentalPricePerHour = baseRentalPricePerHour;
            ImageData = imageData;
        }
    }
}
