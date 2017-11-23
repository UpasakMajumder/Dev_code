using System.ComponentModel.DataAnnotations;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class ProductCategoryImportDto
    {
        [Header(1, "Product Category *")]
        [Required]
        [MaxLength(100)]
        public string ProductCategory { get; set; }

        [Header(2, "Image URL")]
        [MaxLength(200)]
        public string ImageURL { get; set; }

        [Header(4, "Description")]
        public string Description { get; set; }
    }
}