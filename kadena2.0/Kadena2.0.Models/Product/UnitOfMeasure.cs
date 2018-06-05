namespace Kadena.Models.Product
{
    public class UnitOfMeasure
    {
        public static string DefaultUnit => "Each";

        public string Name { get; set; }
        public string ErpCode { get; set; }
        public string LocalizationString { get; set; }
        public bool IsDefault { get; set; }
    }
}
