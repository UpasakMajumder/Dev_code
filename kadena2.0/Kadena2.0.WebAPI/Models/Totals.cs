using System.Collections.Generic;

namespace Kadena.WebAPI.Models
{
    public class Totals
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<Total> Items { get; set; }
    }
}