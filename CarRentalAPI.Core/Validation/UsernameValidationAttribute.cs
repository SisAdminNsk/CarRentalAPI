using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CarRentalAPI.Core.Validation
{
    public class UsernameValidationAttribute : ValidationAttribute
    {
        public UsernameValidationAttribute() { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string userLogin)
            {
                if (IsValidUserLogin(userLogin))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult($"Имя пользователя должно содеражать содержать символы [ a-Z, 0-9, _ ] " +
                    $"и иметь длину от 6 до 32 символов.");
            }

            return new ValidationResult($"Invalid Type for attribute {nameof(UsernameValidationAttribute)}" +
                $" with field {value?.GetType()}");
        }

        private static bool IsValidUserLogin(string userLogin) 
        {
            if (string.IsNullOrWhiteSpace(userLogin))
            {
                return false;
            }

            if (userLogin.Length < 6 || userLogin.Length > 32)
            {
                return false;
            }

            string pattern = @"^[a-zA-Z0-9_]+$"; 

            try 
            {
                return Regex.IsMatch(userLogin, pattern);
            }
            catch(Exception ex)
            {
                return false;
            }            
        }

    }
}
