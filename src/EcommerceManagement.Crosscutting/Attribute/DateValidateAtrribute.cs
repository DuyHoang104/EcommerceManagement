using System;
using System.ComponentModel.DataAnnotations;

public class DateValidateAttribute : ValidationAttribute
{
    private readonly int _minimumValue;

    public DateValidateAttribute(int minimumValue)
    {
        _minimumValue = minimumValue;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is not DateOnly dateValue)
        {
            return new ValidationResult("Invalid date format.");
        }

        var today = DateOnly.FromDateTime(DateTime.Now);
        var minAllowedDate = today.AddYears(-_minimumValue);

        if (dateValue > minAllowedDate)
        {
            var errorMessage = string.IsNullOrEmpty(ErrorMessage) ? $"You must be at least {_minimumValue} years old." : string.Format(ErrorMessage, _minimumValue);
            return new ValidationResult(errorMessage);
        }

        return ValidationResult.Success;
    }
}
