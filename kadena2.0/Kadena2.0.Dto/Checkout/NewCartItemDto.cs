using System;
using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class NewCartItemDto
    {
        public int DocumentId { get; set; }
        public string CustomProductName { get; set; }
        public int Quantity { get; set; }
        public Guid TemplateId { get; set; }
        public Guid ContainerId { get; set; }
        public Dictionary<string, int> Options { get; set; }
    }
}
