namespace Kadena.Old_App_Code.Kadena.Constants
{
    /// <summary>
    /// This Class represents SQl Queries used.
    /// </summary>
    public static class SQLQueries
    {
        public const string shoppingCartCartItems = "Ecommerce.Shoppingcart.GetCartItems";
        public const string distributorCartData = "Ecommerce.Shoppingcart.DistributorCartData";
        public const string loggedInUserCartData = "Ecommerce.Shoppingcart.LoggedInUserCartData";
        public const string getShoppingCartCount = "Ecommerce.Shoppingcart.GetShoppingCartCount";
        public const string getShoppingCartTotal = "Ecommerce.Shoppingcart.GetShoppingCartTotal";
        public const string getPrebuyProductCount = "KDA.Transformations.PrebuyProductCount";
    }
    /// <summary>
    /// This Class represents Transformations used .
    /// </summary>
    public static class TransformationNames
    {
        public const string cartItems = "KDA.Transformations.xCartItems";
    }
    public static class OrderType
    {
        public const string prebuy = "prebuy";
        public const string generalInventory = "general";
    }
    /// <summary>
    /// This class represents orderstatus
    /// </summary>
    public static class OrderStatusConstants
    {
        public const string OrderInProgress = "Submission in progress";
        public const string OrderPlaced = "Submitted";
    }
    /// <summary>
    /// This class represents status querystrings
    /// </summary>
    public static class QueryStringStatus
    {
        public const string Added = "added";
        public const string Updated = "updated";
        public const string Deleted = "deleted";
        public const string OrderScheduleTaskStart = "ordertask";
        public const string OrderSuccess = "ordersuccess";
        public const string OrderFail = "orderfail";
        public const string InvalidCartItems = "invalidcartitems";
    }
    /// <summary>
    /// This class represents shipping options
    /// </summary>
    public class ShippingOption
    {
        public const string Ground = "ground";
    }
    /// <summary>
    /// This class represents SKU Measuring Units
    /// </summary>
    public class SKUMeasuringUnits
    {
        public const string Lb = "Lb";
    }
    /// <summary>
    /// This class represents Scheduled task names
    /// </summary>
    public class ScheduledTaskNames
    {
        public const string PrebuyOrderCreation = "PrebuyOrderCreation";
    }
}