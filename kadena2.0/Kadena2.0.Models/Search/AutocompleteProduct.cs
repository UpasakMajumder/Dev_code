namespace Kadena.Models.Search
{
    public class AutocompleteProduct
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Stock Stock { get; set; }
    }
}
