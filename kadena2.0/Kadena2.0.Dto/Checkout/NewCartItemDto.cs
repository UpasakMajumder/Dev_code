using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Checkout
{
    public class NewCartItemDto
    {
        public int DocumentId { get; set; }
        public int NodeId { get; set; }
        public string CustomProductName { get; set; }
        [Required]
        public int Quantity { get; set; }
        public Guid TemplateId { get; set; }
        public Guid ContainerId { get; set; }
        public Dictionary<string, int> Options { get; set; }
    }
}
