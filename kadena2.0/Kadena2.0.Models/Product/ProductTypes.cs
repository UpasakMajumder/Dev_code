using System.Linq;

namespace Kadena.Models.Product
{
    public class ProductTypes
    {
        public static string POD => "KDA.POD";
        public static string StaticProduct => "KDA.StaticProduct";
        public static string InventoryProduct => "KDA.InventoryProduct";
        public static string ProductWithAddOns => "KDA.ProductWithAddOns";
        public static string MailingProduct => "KDA.MailingProduct";
        public static string TemplatedProduct => "KDA.TemplatedProduct";

        /// <summary>
        /// Checks is Product is of given type
        /// </summary>
        /// <param name="productType">Type string of the product</param>
        /// <param name="testedType">Type to check</param>
        /// <returns></returns>
        public static bool IsOfType(string productType, string checkedType)
        {
            return productType?.Contains(checkedType) ?? false;
        }

        public static string[] GetAll()
        {
            return typeof(ProductTypes)
                .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Select(p => p.GetValue(null))
                .Cast<string>()
                .ToArray();
        }

        public static string Combine(params string[] types)
        {
            return string.Join("|", types);
        }
    }
}