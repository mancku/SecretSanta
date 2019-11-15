using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SecretSantaBindingModels.ValidationAttributes
{
    public class ValueIsEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (string.IsNullOrWhiteSpace(value.ToString()))
                {
                    return ValidationResult.Success;
                }

                var isEmail = Regex.IsMatch(value.ToString(),
                    @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                    RegexOptions.IgnoreCase);

                return isEmail
                    ? ValidationResult.Success
                    : new ValidationResult(this.ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}