using Kadena.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.Models.TableSorting
{
    public static class TableSortingHelper
    {
        public static string OrderByParameterName => "orderby";
        public static string OrderByDirectionParameterName => "orderbydirection";

        public static string DirectionAscending = "asc";
        public static string DirectionDescending = "desc";

        public static string GetOrderBy(string[] columns, string currentUrl)
        {
            var orderBy = ExtractOrderByFromUrl(currentUrl.ToLower());
            if (!columns.Any(c => c.ToLower() == orderBy.Column.ToLower()))
            {
                return string.Empty;
            }

            return $"{orderBy.Column} {orderBy.Direction}".Trim();
        }

        public static OrderBy ExtractOrderByFromUrl(string url)
        {
            url = url.ToLower();
            var parameters = UrlHelper.ParseQueryStringFromUrl(url);

            var orderByColumn = parameters.Get(OrderByParameterName);
            var orderByDirection = parameters.Get(OrderByDirectionParameterName);

            if (orderByDirection != DirectionAscending && orderByDirection != DirectionDescending)
            {
                orderByDirection = string.Empty;
            }

            return new OrderBy(orderByColumn, orderByDirection);
        }
    }
}
