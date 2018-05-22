using System;
using System.ComponentModel.DataAnnotations;

namespace Kadena.Helpers
{
    public static class EnumHelper
    {
        public static string GetDisplayName(this Enum value)
        {
            var enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            var members = enumType.GetMember(enumValue);
            if (members.Length == 0)
            {
                return string.Empty;
            }

            var attrs = members[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attrs.Length == 0)
            {
                return string.Empty;
            }

            var name = ((DisplayAttribute)attrs[0]).Name;
            return name;
        }
    }
}
