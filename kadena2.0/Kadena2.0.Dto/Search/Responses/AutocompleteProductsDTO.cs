using System.Collections.Generic;

namespace Kadena.Dto.Search.Responses
{
    public class AutocompleteProductsDTO
    {
        public string Url { get;set;}
        public IList<AutocompleteProductDTO> Items { get; set; }
        
    }
}
