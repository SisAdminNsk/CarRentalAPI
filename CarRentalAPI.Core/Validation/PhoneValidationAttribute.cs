﻿using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CarRentalAPI.Core.Validation
{
    public class PhoneValidationAttribute : ValidationAttribute
    {
        public PhoneValidationAttribute() { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string userPhone)
            {
                if (IsValidUserPhone(userPhone))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult("Указан неверный номер");
            }

            return new ValidationResult($"Invalid Type for attribute {nameof(PhoneValidationAttribute)}" +
                $" with field {value?.GetType()}");
        }

        private static bool IsValidUserPhone(string userPhone)
        {
            if (string.IsNullOrWhiteSpace(userPhone))
            {
                return false;
            }

            if (userPhone.Length != 11)
            {
                return false;
            }

            string pattern = @"^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$";

            try
            {
                return Regex.IsMatch(userPhone, pattern);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}