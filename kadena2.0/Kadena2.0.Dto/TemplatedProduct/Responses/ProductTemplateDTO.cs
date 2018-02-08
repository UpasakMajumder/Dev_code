using System;

namespace Kadena.Dto.TemplatedProduct.Responses
{
    public class ProductTemplateDTO
    {
        public Guid TemplateId { get; set; }
        public string ProductName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string EditorUrl { get; set; }
        public string Image { get; set; }
    }
}