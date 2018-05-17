namespace Kadena.Models.Product
{
    public class ProductAvailability
    {
        public static string OutOfStock => "outofstock";
        public static string Unavailable => "unavailable";
        public static string Available => "available";

        public string Type { get; set; }
        public string Text { get; set; }
    }
}