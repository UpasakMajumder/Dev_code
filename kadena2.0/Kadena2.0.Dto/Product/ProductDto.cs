namespace Kadena.Dto.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsFavourite { get; set; }
        public BorderDto Border { get; set; }
    }
}
