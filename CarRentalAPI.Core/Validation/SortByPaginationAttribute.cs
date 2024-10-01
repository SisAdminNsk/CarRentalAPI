using CSharpFunctionalExtensions;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Core.Validation
{

    public class SortByPaginationAttribute<Entity> : ValidationAttribute where Entity : Entity<Guid>
    {
        public SortByPaginationAttribute() { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string sortByField)
            {
                if (IsSortableField(sortByField))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult($"field with name {nameof(sortByField)} can't be sortable." +
                   $"specify a {nameof(SortableAttribute)} attribute above the field that should be sortable.");
            }

            return new ValidationResult($"Invalid Type for attribute {nameof(SortByPaginationAttribute<Entity>)}" +
                $" with field {value?.GetType()}");
        }
        static private bool IsSortableField(string fieldName)
        {
            var propertyInfo = typeof(Entity).GetProperties()
                .FirstOrDefault(p => p.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase) &&
                                     Attribute.IsDefined(p, typeof(SortableAttribute)));

            return propertyInfo != null;
        }
    }
}
