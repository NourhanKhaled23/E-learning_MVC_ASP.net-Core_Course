using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Validation
{
    public class LessThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public LessThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // 1. احصل على قيمة الحقل الحالي (MinDegree)
            if (value == null) return ValidationResult.Success;

            double currentValue;
            if (!double.TryParse(value.ToString(), out currentValue))
                return new ValidationResult("Invalid value format.");

            // 2. احصل على قيمة الحقل المراد المقارنة معه (FullDegree)
            var propertyInfo = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (propertyInfo == null)
                return new ValidationResult($"Property '{_comparisonProperty}' not found.");

            var comparisonValueObj = propertyInfo.GetValue(validationContext.ObjectInstance);
            if (comparisonValueObj == null) 
                return ValidationResult.Success; // Leave other validation to Required attribute

            double comparisonValue = (double)comparisonValueObj;

            // 3. قاعدة التحقق
            if (currentValue >= comparisonValue)
            {
                return new ValidationResult(ErrorMessage ?? $"The minimum degree must be less than the total degree ({comparisonValue}).");
            }

            return ValidationResult.Success;
        }
    }
}
