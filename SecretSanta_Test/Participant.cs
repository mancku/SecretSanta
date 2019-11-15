using System;

namespace SecretSanta_Test
{
    public class Participant : IEquatable<Participant>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMailAddress { get; set; }
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

            return string.Equals(this.FirstName, other.FirstName)
                   && string.Equals(this.LastName, other.LastName)
                   && string.Equals(this.EMailAddress, other.EMailAddress)
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
                var hashCode = (this.FirstName != null ? this.FirstName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.LastName != null ? this.LastName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.EMailAddress != null ? this.EMailAddress.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.PhoneNumber != null ? this.PhoneNumber.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return
                $"Full name: {this.FirstName} {this.LastName}, Email: {this.EMailAddress}, Phone: {this.PhoneNumber}";
        }
    }
}
