using System;
using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_benenatti.Models
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var currentValue = (DateTime?)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
                throw new ArgumentException("La propiedad de comparación no existe");

            var comparisonValue = (DateTime?)property.GetValue(validationContext.ObjectInstance);

            if (currentValue != null && comparisonValue != null && currentValue <= comparisonValue)
            {
                return new ValidationResult(ErrorMessage ?? $"La fecha debe ser mayor a {_comparisonProperty}");
            }

            return ValidationResult.Success;
        }
    }
}
