using System.ComponentModel.DataAnnotations;

namespace SecretSantaBindingModels.ValidationAttributes
{
    public class OtherPropertyIsInformedIfThisIsNotAttribute : ValidationAttribute
    {
        public OtherPropertyIsInformedIfThisIsNotAttribute(string otherPropertyName)
        {
            this.OtherPropertyName = otherPropertyName;
        }

        public string OtherPropertyName { get; set; }

        protected override ValidationResult IsValid(object firstValue, ValidationContext validationContext)
        {
            if (firstValue == null)
            {
                var secondProperty = validationContext
                    .ObjectType
                    .GetProperty(this.OtherPropertyName);

                var secondValue = secondProperty?.GetValue(validationContext.ObjectInstance, null);
                if (secondValue == null)
                {
                    return new ValidationResult(this.ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}