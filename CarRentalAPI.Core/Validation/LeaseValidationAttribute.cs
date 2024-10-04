using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Core.Validation
{
    public class LeaseValidationAttribute : ValidationAttribute
    {
        public LeaseValidationAttribute()  {}

        enum LeaseValidationStatus
        {
            Ok = 0,
            LessThanServerTime = 1,
            MinimalOrderTimeError = 2,
            StartOfLeaseLaterThanEndOfLease = 3
        }

        static private readonly int _minimalLeaseTimeInHours = 1;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is LeaseDateTime leaseDateTime)
            {
                var leaseValidationResult = isLeaseDateTimeValid(leaseDateTime);

                if (leaseValidationResult == LeaseValidationStatus.Ok)
                {
                    return ValidationResult.Success;
                }

                switch(leaseValidationResult)
                {
                    case LeaseValidationStatus.LessThanServerTime:
                        return new ValidationResult($"Server time is: {DateTime.UtcNow}. With server time conflict");

                    case LeaseValidationStatus.MinimalOrderTimeError:
                        return new ValidationResult($"Minimal order time value is: {_minimalLeaseTimeInHours} hour. " +
                            $"Start of lease value is: {leaseDateTime.StartOfLease}," +
                            $" End of lease value is: {leaseDateTime.EndOfLease}");

                    case LeaseValidationStatus.StartOfLeaseLaterThanEndOfLease:
                        return new ValidationResult($"Start of lease {leaseDateTime.StartOfLease} is later than " +
                            $"End of lease {leaseDateTime.EndOfLease}.");

                    default:
                        throw new NotImplementedException("Not all status checking in this switch-case-block.");
                }
            }

            return new ValidationResult($"Invalid Type for attribute {nameof(LeaseValidationAttribute)}" +
                $" with field {value?.GetType()}");
        }

        static private LeaseValidationStatus isLeaseDateTimeValid(LeaseDateTime leaseDateTime)
        {
            if(DateTime.UtcNow < leaseDateTime.StartOfLease)
            {
                if(leaseDateTime.StartOfLease > leaseDateTime.EndOfLease)
                {
                    return LeaseValidationStatus.StartOfLeaseLaterThanEndOfLease;
                }

                if(leaseDateTime.StartOfLease.AddHours(_minimalLeaseTimeInHours) <= leaseDateTime.EndOfLease)
                {
                    return LeaseValidationStatus.Ok;
                }

                return LeaseValidationStatus.MinimalOrderTimeError;
            }

            return LeaseValidationStatus.LessThanServerTime;
        }
    }
}
