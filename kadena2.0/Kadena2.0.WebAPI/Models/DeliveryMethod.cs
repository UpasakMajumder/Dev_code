using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Models
{
    public class DeliveryMethod
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool Opened { get; set; }
        public bool Disabled { get; set; }
        public string PricePrefix { get; set; }
        public string Price { get; set; }
        public string DatePrefix { get; set; }
        public string Date { get; set; }
        public List<DeliveryService> items { get; set; }
    }
}