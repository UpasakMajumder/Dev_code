using AutomatedTests.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.KenticoApi.Objects
{
    public class ProductCategory : CmsDocument
    {
        public string ProductCategoryDescription { get; set; }
        public string ProductCategoryTitle { get; set; }
        public string ProductCategoryImage { get; set; }
        public int ProductCategoryID { get; set; }

        public ProductCategory Init()
        {
            ProductCategoryTitle = StringHelper.RandomString(7);
            ProductCategoryDescription = Lorem.Paragraph(4);
            NodeClassID = 5357;
            //DocumentPageTemplateID = 25752;

            return this;
        }
    }

}
