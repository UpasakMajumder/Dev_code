using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kadena.Models.Checkout
{
    public class NewCartItem
    {
        public int DocumentId { get; set; }

        [MaxLength(40)]
        public string CustomProductName { get; set; }

        public int Quantity { get; set; }
        
        public Guid TemplateId { get; set; }

        public Guid ContainerId { get; set; }

        public Dictionary<string, int> Options { get; set; }
    }
}
