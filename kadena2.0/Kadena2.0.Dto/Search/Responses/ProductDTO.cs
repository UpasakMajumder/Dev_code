using System.Collections.Generic;

namespace Kadena.Dto.Search.Responses
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public bool IsFavourite { get; set; }
        public string ImgUrl { get; set; }
        public IList<string> Breadcrumbs { get; set; }
        public string Title { get; set; }
        public StockDTO Stock { get; set; }
        public UseTemplateBtnDTO UseTemplateBtn { get; set; }
    }
}
