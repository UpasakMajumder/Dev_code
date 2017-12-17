namespace Kadena.Models.CustomerData
{
    public class CartDistributorItem
    {
        public int AddressID { get; set; }
        public string AddressPersonalName { get; set; }
        public bool IsSelected { get; set; }
        public int ShoppingCartID { get; set; }
        public int SKUID { get; set; }
        public int SKUUnits { get; set; }
    }
}
