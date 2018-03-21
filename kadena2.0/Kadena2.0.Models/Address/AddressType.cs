using System.Linq;

namespace Kadena.Models
{
    public sealed class AddressType
    {
        public static AddressType Billing { get; } = new AddressType("Billing");
        public static AddressType Shipping { get; } = new AddressType("Shipping");

        public string Code { get; private set; }

        private AddressType(string code)
        {
            Code = code;
        }

        public static AddressType[] GetAll()
        {
            return typeof(AddressType)
                .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(AddressType))
                .Select(p => p.GetValue(null))
                .Cast<AddressType>()
                .ToArray();
        }

        public static AddressType FromString(string code)
        {
            return GetAll().First(at => at.Code == code);
        }

        public override string ToString()
        {
            return Code;
        }

        public override bool Equals(object obj)
        {
            var other = obj as AddressType;
            if (other == null)
            {
                return false;
            }

            return Code == other.Code;
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }

        public static implicit operator string(AddressType at)
        {
            return at.Code;
        }
    }
}