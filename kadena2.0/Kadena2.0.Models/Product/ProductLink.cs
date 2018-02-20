using System;

namespace Kadena.Models.Product
{
    public class ProductLink
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsFavourite { get; set; }
        public string ParentPath { get; set; }
        public Border Border { get; set; }
        public int Order { get; set; }
        public void SetBorderInfo(bool bordersEnabledOnSite, bool borderEnabledOnParentCategory, string borderStyle)
        {
            if (Border == null)
            {
                Border = new Border() { Exists = false };
                return;
            }

            Border.Exists = Border.Exists && bordersEnabledOnSite && borderEnabledOnParentCategory;
            Border.Value = (Border.Exists ? borderStyle : string.Empty);
        }
    }
}