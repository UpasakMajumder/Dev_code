using CMS.DocumentEngine;

namespace Kadena2.WebAPI.KenticoProviders.Classes
{
    public class CampaignsProductNodeWrapper
    {
        private TreeNode productNode;

        public static string CLASS_NAME => "KDA.CampaignsProduct";

        public double ProductWeight
        {
            get => productNode.GetDoubleValue("ProductWeight", 0);
            set => productNode.SetValue("ProductWeight", value);
        }

        public int NodeSKUID
        {
            get => productNode.GetIntegerValue("NodeSKUID", 0);
            set => productNode.SetValue("NodeSKUID", value);
        }

        public CampaignsProductNodeWrapper(TreeNode productNode)
        {
            this.productNode = productNode;
        }

        public ProductClass ToProduct()
        {
            if (productNode?.ClassName != CLASS_NAME)
            {
                return null;
            }

            return new ProductClass
            {
                NodeSKUID = NodeSKUID,
                SKUWeight = ProductWeight,
                SKUNeedsShipping = false
            };
        }
    }
}