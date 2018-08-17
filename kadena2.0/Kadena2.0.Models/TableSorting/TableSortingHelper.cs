using Kadena.Helpers;
using System;
using System.Linq;

namespace Kadena.Models.TableSorting
{
    public static class TableSortingHelper
    {
        public static string OrderByParameterName => "orderby";
        public static string OrderByDirectionParameterName => "orderbydirection";

        public static string DirectionAscending => "asc";
        public static string DirectionDescending => "desc";
        public static string DirectionEmpty => "no";


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
            var parameters = UrlHelper.ParseQueryStringFromUrl(url.ToLower());

            var orderByColumn = parameters.Get(OrderByParameterName) ?? string.Empty;
            var orderByDirection = parameters.Get(OrderByDirectionParameterName);

            if (orderByDirection != DirectionAscending && orderByDirection != DirectionDescending)
            {
                orderByDirection = string.Empty;
            }

            return new OrderBy(orderByColumn, orderByDirection);
        }

        public static string GetColumnDirection(string column, string currentUrl)
        {
            column = column.ToLower();
            var orderBy = ExtractOrderByFromUrl(currentUrl);
            if (orderBy.Column == column)
            {
                if (orderBy.Direction == DirectionDescending)
                {
                    return DirectionDescending;
                }
                else
                {
                    return DirectionAscending;
                }
            }

            return DirectionEmpty;
        }

        public static string GetColumnURL(string column, string currentUrl)
        {
            column = column.ToLower();
            var orderBy = ExtractOrderByFromUrl(currentUrl.ToLower());
            var direction = DirectionAscending;
            if (orderBy.Column == column && orderBy.Direction != DirectionDescending)
            {
                direction = DirectionDescending;
            }

            currentUrl = UrlHelper.SetQueryParameter(currentUrl, OrderByParameterName, column);
            currentUrl = UrlHelper.SetQueryParameter(currentUrl, OrderByDirectionParameterName, direction);
            return currentUrl;
        }
    }
}
