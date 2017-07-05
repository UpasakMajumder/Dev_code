using System.Collections.Generic;

namespace Kadena.Dto.Search.Responses
{
    public class AutocomletePagesDTO
    {
        public string Url { get; set; }
        public IList<AutocompletePageDTO> Items { get; set; }
    }
}
