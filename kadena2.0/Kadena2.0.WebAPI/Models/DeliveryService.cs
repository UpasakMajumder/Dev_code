using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Models
{
    public class DeliveryService
    {
        public int Id { get; set; }
        public int CarrierId { get; set; }
        public string Title { get; set; }
        public bool Checked { get; set; }
        public string PricePrefix { get; set; }
        public string Price { get; set; }
        public string DatePrefix { get; set; }
        public string Date { get; set; }
        public bool Disabled { get; set; }
    }
}