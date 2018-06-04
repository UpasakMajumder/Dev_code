using System.Linq;

namespace Kadena.Models.Product
{
    public class PricingModel
    {
        public static string Dynamic => "Dynamic";
        public static string Tiered => "Tiered";

        public static string[] GetAll()
        {
            return typeof(PricingModel)
                .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Select(p => p.GetValue(null))
                .Cast<string>()
                .ToArray();
        }

        public static string GetDefault()
        {
            return Dynamic;
        }
    }
}
