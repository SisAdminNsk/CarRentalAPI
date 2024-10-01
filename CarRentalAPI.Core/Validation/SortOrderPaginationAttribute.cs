using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Core.Validation
{
    public class SortOrderPaginationAttribute : ValidationAttribute
    {
        public SortOrderPaginationAttribute() { }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string sortOrder)
            {
                if (SortOrdersPagination.IsSortOrderAvailable(sortOrder))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult($"BadRequest. value {sortOrder} is not a sorting method.");
            }

            return new ValidationResult($"Invalid Type for attribute {nameof(SortOrderPaginationAttribute)}" +
                $" with field {value?.GetType()}");
        }
    }
}
