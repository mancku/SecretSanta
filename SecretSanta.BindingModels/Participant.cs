using System;
using System.ComponentModel.DataAnnotations;
using SecretSanta.BindingModels.ValidationAttributes;

namespace SecretSanta.BindingModels
{
    public class Participant : IEquatable<Participant>
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [ValueIsEmail]
        [OtherPropertyIsInformedIfThisIsNot("PhoneNumber",
            ErrorMessage = "The 'PhoneNumber' field must contain information when the field 'Email' is empty.")]
        public string Email { get; set; }

        [MinLength(9, ErrorMessage = "The phone number is too short")]
        public string PhoneNumber { get; set; }

        public bool Equals(Participant other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(this.Name, other.Name)
                   && string.Equals(this.Email, other.Email)
                   && string.Equals(this.PhoneNumber, other.PhoneNumber);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == this.GetType()
                   && this.Equals((Participant)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Email != null ? this.Email.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.PhoneNumber != null ? this.PhoneNumber.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return
                $"Full name: {this.Name}, Email: {this.Email}, Phone: {this.PhoneNumber}";
        }
    }
}
