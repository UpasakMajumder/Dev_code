using System.Collections.Generic;

namespace Kadena.Dto.Search.Responses
{
    public class SearchResultPageResponseDTO
    {
        public IList<ProductDTO> products { get; set; }
        public IList<PageDTO> pages { get; set; }
    }
}
