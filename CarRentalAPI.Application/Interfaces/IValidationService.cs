using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Application.Interfaces
{
    public interface IValidationService
    {
        public List<ValidationResult> Validate(object? instance);
    }
}
