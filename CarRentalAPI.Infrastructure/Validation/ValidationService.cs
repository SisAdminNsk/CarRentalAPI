using CarRentalAPI.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Infrastructure.Validation
{
    public class ValidationService : IValidationService
    {
        public List<ValidationResult> Validate(object? instance)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(instance);
            Validator.TryValidateObject(instance, context, validationResults, true);

            return validationResults;
        }
    }
}
