using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DateValidateAttribute : ValidationAttribute
{
    private readonly int _minimumAge;

    public DateValidateAttribute(int minimumAge)
    {
        _minimumAge = minimumAge;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is not DateOnly dateOfBirth)
        {
            return new ValidationResult("Invalid date format.");
        }

        var today = DateOnly.FromDateTime(DateTime.Now);
        var minAllowedDate = today.AddYears(-_minimumAge);

        if (dateOfBirth > minAllowedDate)
        {
            return new ValidationResult($"You must be at least {_minimumAge} years old.");
        }

        return ValidationResult.Success;
    }
}
