using CarRentalAPI.Core.Validation;
using CSharpFunctionalExtensions;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Core
{
    public class CarsharingUser : Entity<Guid>
    {
        public User User { get; set; }
        public List<CarOrder> Orders { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name {  get; set; }
        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }

        [Required]
        [MaxLength(100)]
        public string Patronymic { get; set; }
        [Required]
        [Range(18, 100)]
        public int Age { get; set; }
        [Required]
        [PhoneValidation]
        public string Phone { get; set; }

        public CarsharingUser()
        {
            Id = Guid.NewGuid();
        }
        public CarsharingUser(User user, int age, string name, string surname, string patronymic, string phone) 
        {
            User = user;
            Age = age;
            Surname = surname;
            Patronymic = patronymic;
            Name = name;
            Phone = phone;
        }
    }
}
