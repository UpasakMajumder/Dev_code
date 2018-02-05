using System;

namespace Kadena.Models.TemplatedProduct
{
    public class ProductTemplate
    {
        public Guid TemplateId { get; set; }
        public string Image { get; set; }
        public string ProductName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string EditorUrl { get; set; }
    }
}