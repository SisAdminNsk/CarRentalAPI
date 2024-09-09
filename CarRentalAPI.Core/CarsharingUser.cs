using CSharpFunctionalExtensions;

namespace CarRentalAPI.Core
{
    public class CarsharingUser : Entity<Guid>
    {
        public User User { get; set; }
        public List<CarOrder> Orders { get; set; }
        public string Name {  get; set; }
        public int Age { get; set; }

        public CarsharingUser()
        {
            Id = Guid.NewGuid();
        }
        public CarsharingUser(User user, int age, string name) 
        {
            User = user;
            Age = age;
            Name = name;
        }
    }
}
