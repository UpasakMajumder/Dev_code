using AutomatedTests.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.KenticoApi.Objects
{
    public class Product : CmsDocument
    {
        public object ProductOrderQuantities { get; set; }
        public string ProductBindery { get; set; }
        public string ProductMachineType { get; set; }
        public string ProductSheetSize { get; set; }
        public object ProductDigitalPrinting { get; set; }
        public string ProductPaper { get; set; }
        public string ProductType { get; set; }
        public string ProductTrimSize { get; set; }
        public string ProductFinishedSize { get; set; }
        public string ProductColor { get; set; }
        public int ProductID { get; set; }
        public string ProductShipTime { get; set; }
        public string ProductThumbnail { get; set; }
        public string ProductProductionTime { get; set; }
        public string ProductShippingCost { get; set; }
        public string ProductDynamicPricing { get; set; }
        public string ProductCustomerReferenceNumber { get; set; }
        public string ProductArtworkLocation { get; set; }
        public int ProductNumberOfItemsInPackage { get; set; }
        public string ProductChiliTemplateID { get; set; }

        public Product Init()
        {
            var name = Lorem.Word();
            DocumentPageTemplateID = 25820;
            DocumentName = name;
            NodeName = name;
            //DocumentSKUName = name;
            ProductArtworkLocation = Lorem.Word();
            ProductChiliTemplateID = Lorem.Word();
            ProductNumberOfItemsInPackage = Lorem.RandomNumber(1, 10);
            ProductType = "KDA.StaticProduct";
            NodeClassID = 5356;

            return this;
        }
    }

    public class DynamicPricing
    {
        public string MinVal { get; set; }
        public string MaxVal { get; set; }
        public string Price { get; set; }
    }
}
