namespace Kadena.Models.TemplatedProduct
{
    public class ProductTemplates
    {
        public string Title { get; set; }
        public string OpenInDesignBtn { get; set; }
        public ProductTemplatesHeader[] Header { get; set; }
        public ProductTemplate[] Data { get; set; }
    }
}