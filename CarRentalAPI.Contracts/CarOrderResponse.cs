namespace CarRentalAPI.Contracts
{
    public class CarOrderResponse
    {
        public Guid Id { get; set; }
        public DateTime StartOfLease { get; set; }
        public DateTime EndOfLease { get; set; }
        public string Name {  get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Phone {  get; set; }
        public string Comment { get; set; }
        public decimal Price { get; set; }

        public CarOrderResponse(
            Guid id,
            DateTime startOfLease,
            DateTime endOfLease,
            string name,
            string surname,
            string patronymic,
            string phone,
            string comment,
            decimal price)
        {
            Id = id;
            StartOfLease = startOfLease;
            EndOfLease = endOfLease;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Phone = phone;
            Comment = comment;
            Price = price;
        }
    }
}
