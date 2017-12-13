namespace Kadena.Old_App_Code.Kadena.Constants
{
    /// <summary>
    /// This Class represents Stored Procedures used.
    /// </summary>
    public static class StoredProcedures
    {
        public const string distributorCartData = "Proc_Custom_DistributorCartData";
        public const string loggedInUserCartData = "Proc_Custom_LoggedInUserCartData";
        public const string getShoppingCartCount = "Proc_Custom_GetShoppingCartCount";
        public const string getShoppingCartTotal = "Proc_Custom_GetShoppingCartTotal";
    }
    /// <summary>
    /// This Class represents SQl Queries used.
    /// </summary>
    public static class SQLQueries
    {
        public const string shoppingCartCartItems = "Ecommerce.Shoppingcart.GetCartItems";
    }
    /// <summary>
    /// This Class represents Transformations used .
    /// </summary>
    public static class TransformationNames
    {
        public const string cartItems = "KDA.Transformations.xCartItems";
    }
}