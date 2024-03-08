using System.ComponentModel.DataAnnotations;

namespace Entities.CustomValidationAttributes
{
    public class DateTimeRange : ValidationAttribute
    {
        private readonly DateTime _minDate;
        private readonly DateTime _maxDate;

        public DateTimeRange(string minDate, string? maxDate = null)
        {
            _minDate = DateTime.Parse(minDate);
            if(maxDate == null) { _maxDate = DateTime.MaxValue; }
            else { _maxDate = DateTime.Parse(maxDate); } 
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime >= _minDate && dateTime <= _maxDate)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult($"The date must be between {_minDate.ToShortDateString()} and {_maxDate.ToShortDateString()}.");
            }
            return new ValidationResult("The value is not a valid DateTime.");
        }
    }
}
