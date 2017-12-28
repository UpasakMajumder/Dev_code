namespace Kadena.Helpers
{
    public static class StringHelpers
    {
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return char.ToLower(value[0]) + value.Substring(1);
        }
    }
}