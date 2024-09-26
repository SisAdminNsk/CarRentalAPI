namespace CarRentalAPI.Contracts
{
    public class CarsharingUserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public CarsharingUserResponse()
        {

        }

        public CarsharingUserResponse(
            Guid id,
            string name,
            string surname,
            string patronymic,
            string email,
            string phone,
            int age) 
        { 
            Id = id;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Email = email;
            Phone = phone;
            Age = age;
        }
    }
}
