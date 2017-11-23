using System.ComponentModel.DataAnnotations;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class ProductCategoryImportDto
    {
        [Header(0, "Product Category *")]
        [Required]
        [MaxLength(100)]
        public string ProductCategory { get; set; }

        [Header(1, "Image URL")]
        [MaxLength(200)]
        public string ImageURL { get; set; }

        [Header(2, "Description")]
        public string Description { get; set; }
    }
}