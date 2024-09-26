using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Core.Validation
{
    public class CarOrderStatusValidationAttribute : ValidationAttribute
    {
        public CarOrderStatusValidationAttribute() { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string carOrderStatus)
            {

                if (CarOrdersStatus.IsStatusAvailable(carOrderStatus)){

                    return ValidationResult.Success;
                }
            }

            return new ValidationResult($"Invalid Type for attribute " +
                $"{nameof(CarOrderStatusValidationAttribute)} with field {value?.GetType()}");
        }
    }
}
