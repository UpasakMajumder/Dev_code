using CMS.DocumentEngine;

namespace Kadena2.WebAPI.KenticoProviders.Classes
{
    public class ProductClassWrapper
    {
        private TreeNode productNode;

        public static string CLASS_NAME => "KDA.Product";

        public double SKUWeight
        {
            get { return productNode.GetDoubleValue("SKUWeight", 0); }
            set { productNode.SetValue("SKUWeight", value); }
        }
        public bool SKUNeedsShipping
        {
            get { return productNode.GetBooleanValue("SKUNeedsShipping", false); }
            set { productNode.SetValue("SKUNeedsShipping", value); }
        }
        public int NodeSKUID
        {
            get { return productNode.GetIntegerValue("NodeSKUID", 0); }
            set { productNode.SetValue("NodeSKUID", value); }
        }

        public TreeNode TreeNode => productNode;

        public ProductClassWrapper(TreeNode productNode)
        {
            this.productNode = productNode;
        }

        public virtual ProductClass ToProduct()
        {
            if (productNode?.ClassName != CLASS_NAME)
            {
                return null;
            }

            return new ProductClass
            {
                NodeSKUID = NodeSKUID,
                SKUNeedsShipping = SKUNeedsShipping,
                SKUWeight = SKUWeight
            };
        }
    }
}