using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CarRentalAPI.Core.Validation
{
    public class UserPasswordValidationAttribute : ValidationAttribute
    {
        public UserPasswordValidationAttribute() { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string userPassword)
            {
                if (IsValidUserPassword(userPassword))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult($"Пароль должен иметь длину от 8 до 20 символов, " +
                    $"иметь хотя бы одну заглавную букву, любую цифру и специальный символ");
            }

            return new ValidationResult($"Invalid Type for attribute {nameof(UserPasswordValidationAttribute)}" +
               $" with field {value?.GetType()}");
        }

        private static bool IsValidUserPassword(string userPassword)
        {
            if (userPassword.Length < 8 || userPassword.Length > 20)
            {
                return false;
            }

            try
            {
                if (!Regex.IsMatch(userPassword, @"[A-Z]"))
                {
                    return false;
                }

                if (!Regex.IsMatch(userPassword, @"[a-z]"))
                {
                    return false;
                }

                if (!Regex.IsMatch(userPassword, @"[0-9]"))
                {
                    return false;
                }

                if (!Regex.IsMatch(userPassword, @"[\W_]"))
                {
                    return false;
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }           
        }
    }
}
