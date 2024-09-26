using CarRentalAPI.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Contracts
{
    public class CreateCarsharingUserRequest
    {
        public Guid UserId { get; set; }
        [CarsharingUserInitialsValidation]
        public string Name { get; set; }
        [CarsharingUserInitialsValidation]
        public string Surname { get; set; }
        [CarsharingUserInitialsValidation]
        public string Patronymic { get; set; }
        [Required]
        [Range(0, 100)]
        public int Age { get; set; }
        [PhoneValidation]
        public string Phone { get; set; }
        public CreateCarsharingUserRequest()
        {

        }

        public CreateCarsharingUserRequest(

            Guid userId,
            string name,
            string surname,
            string patronymic,
            int age,
            string phone)
        {
            Phone = phone;
            UserId = userId;
            Patronymic = patronymic;
            Surname = surname;
            Age = age;
            Name = name;
        }
    }
}
