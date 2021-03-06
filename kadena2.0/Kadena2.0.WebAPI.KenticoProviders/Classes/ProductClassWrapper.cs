﻿using CMS.DocumentEngine;

namespace Kadena2.WebAPI.KenticoProviders.Classes
{
    public class ProductClassWrapper
    {
        private TreeNode productNode;

        public static string CLASS_NAME => "KDA.Product";

        public double ProductSKUWeight
        {
            get { return productNode.GetDoubleValue("ProductSKUWeight", 0); }
            set { productNode.SetValue("ProductSKUWeight", value); }
        }
        public bool ProductSKUNeedsShipping
        {
            get { return productNode.GetBooleanValue("ProductSKUNeedsShipping", false); }
            set { productNode.SetValue("ProductSKUNeedsShipping", value); }
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
                SKUNeedsShipping = ProductSKUNeedsShipping,
                SKUWeight = ProductSKUWeight
            };
        }
    }
}