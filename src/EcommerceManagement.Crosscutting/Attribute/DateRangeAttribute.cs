using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DateRangeAttribute : ValidationAttribute, IClientModelValidator
{
    private readonly DateOnly _minDate;
    private readonly DateOnly _maxDate;

    public DateRangeAttribute(int yearStart, int yearEnd)
    {
        _minDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-yearStart));
        _maxDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-yearEnd));
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is not DateOnly date)
        {
            return new ValidationResult("Invalid date format.");
        }

        if (date < _minDate || date > _maxDate)
        {
            return new ValidationResult($"Date must be between {_minDate:yyyy-MM-dd} and {_maxDate:yyyy-MM-dd}.");
        }

        return ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes["min"] = _minDate.ToString("yyyy-MM-dd");
        context.Attributes["max"] = _maxDate.ToString("yyyy-MM-dd");
    }
}
