using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CarRentalAPI.Core.Validation
{
    public class CarsharingUserInitialsValidationAttribute : ValidationAttribute
    {
        public CarsharingUserInitialsValidationAttribute() { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string initials)
            {
                if (ValidateInitials(initials))
                {

                    return ValidationResult.Success;
                }

                return new ValidationResult($"Инициалы должны сотоять только из букв и иметь длину не меньше 2 символов, " +
                    $"но получено: {value}");
            }

            return new ValidationResult($"Invalid Type for attribute " +
                $"{nameof(CarsharingUserInitialsValidationAttribute)} with field {value?.GetType()}");
        }

        private static bool ValidateInitials(string initials)
        {
            if (string.IsNullOrWhiteSpace(initials) || initials.Length < 2 || initials.Length > 100)
            {
                return false;
            }

            Regex regex = new Regex(@"^[А-Яа-яЁёA-Za-z]+$");

            return regex.IsMatch(initials);
        }

    }
}
