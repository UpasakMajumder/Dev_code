using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.Models.Favorites
{
    public class FavoriteProduct
    {
        public int ID { get; set; }
        public int SiteID { get; set; }
        public int UserID { get; set; }
        public int ProductDocumentID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
