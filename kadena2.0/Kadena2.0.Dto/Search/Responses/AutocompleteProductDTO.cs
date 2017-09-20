using System.Collections.Generic;

namespace Kadena.Dto.Search.Responses
{
    public class AutocompleteProductDTO
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public StockDTO Stock { get; set; }
    }
}
