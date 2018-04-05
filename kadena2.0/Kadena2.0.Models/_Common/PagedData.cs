using System.Collections.Generic;

namespace Kadena.Models.Common
{
    public class PagedData<T>
    {
        public Pagination Pagination { get; set; }
        public List<T> Data { get; set; }
        public static PagedData<T> Empty()
            => new PagedData<T>
            {
                Data = new List<T>(),
                Pagination = new Pagination
                {
                    CurrentPage = 1,
                    PagesCount = 1,
                    RowsCount = 0,
                    RowsOnPage = 1
                }
            };
    }
}
