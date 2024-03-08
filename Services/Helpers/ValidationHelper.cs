using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
    internal class ValidationHelper
    {
        internal static void ValidateModel(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(obj, validationContext, validationResults, true))
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
