namespace Kadena.WebAPI.Models.Search
{
    public class AutocompleteResponse
    {
        public AutocompleteProducts Products { get; set; }
        public AutocomletePages Pages { get; set; }

        public string Message { get; set; } = "Sorry, no results found";
    }
}
