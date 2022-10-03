namespace Email.Core.Models
{
    public class Address
    {
        public string Name { get; }
        public string EmailAddress { get; }

        public Address(string emailAddress, string name = null)
        {
            Name = name;
            EmailAddress = emailAddress;
        }

        public override string ToString()
        {
            return Name == null ? EmailAddress : $"{Name} <{EmailAddress}>";
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ EmailAddress.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                var otherAddress = (Address)obj;
                return this.EmailAddress == otherAddress.EmailAddress && this.Name == otherAddress.Name;
            }
        }
    }
}