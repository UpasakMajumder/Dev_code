using System;

namespace Kadena.WebAPI.Models.Search
{
    public class AutocompleteResponse
    {
        public AutocompleteProducts Products { get; set; }
        public AutocompletePages Pages { get; set; }

        public string Message { get; set; }

        public void UpdateNotFoundMessage(string notFoundMessage)
        {
            Message = (((Products?.Items?.Count ?? 0) == 0) && ((Pages?.Items?.Count ?? 0) == 0)) 
                ?  notFoundMessage 
                : string.Empty;
        }
    }
}
