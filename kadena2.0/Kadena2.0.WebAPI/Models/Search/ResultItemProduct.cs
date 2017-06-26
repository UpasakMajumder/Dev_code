using System.Collections.Generic;

namespace Kadena.WebAPI.Models.Search
{
    public class ResultItemProduct
    {
        public int Id { get; set; }
        public bool IsFavourite { get; set; }
        public string ImgUrl { get; set; }
        public IList<string> Breadcrumbs { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public Stock Stock { get; set; }
        public UseTemplateBtn UseTemplateBtn { get; set; }
    }
}
