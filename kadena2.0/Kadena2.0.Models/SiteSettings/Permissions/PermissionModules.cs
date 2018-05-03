namespace Kadena.Models.SiteSettings.Permissions
{
    public class KadenaOrders
    {
        public string ApproveOrders => "KDA_ApproveOrders";
        public string SeePrices => "KDA_SeePrices";
        public string SeeAllOrders => "KDA_SeeAllOrders";
        public string CanDownloadHiresPdf => "KDA_CanDownloadHiresPdf";

        public static implicit operator string(KadenaOrders kadenaOrders)
        {
            return "Kadena_Orders";
        }
    }

    public class KadenaUserSettings
    {
        public string ModifyShippingAddress => "KDA_ModifyShippingAddress";

        public static implicit operator string(KadenaUserSettings kadenaOrders)
        {
            return "Kadena_User_Settings";
        }
    }
}
