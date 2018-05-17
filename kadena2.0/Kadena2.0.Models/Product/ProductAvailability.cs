namespace Kadena.Models.Product
{
    public class ProductAvailability
    {
        public static string OutOfStock => "OutOfStock";
        public static string Unavailable => "Unavailable";
        public static string Available => "Available";

        public string Type { get; set; }
        public string Text { get; set; }
    }
}