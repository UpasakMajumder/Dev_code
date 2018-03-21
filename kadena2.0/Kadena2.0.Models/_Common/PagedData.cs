using System.Collections.Generic;

namespace Kadena.Models.Common
{
    public class PagedData<T>
    {
        public Pagination Pagination { get; set; }
        public List<T> Data { get; set; }
    }
}
