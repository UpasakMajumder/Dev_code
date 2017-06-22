using System.Runtime.Serialization;
using System.Linq;
using CMS.Helpers;
using Kadena.Dto.Order;

namespace Kadena.Old_App_Code.Kadena.Orders
{
    public class OrderHistoryData : OrderDto
    {
        public string ItemsString
        {
            get
            {
                if (Items != null && Items.ToList().Count > 0)
                {
                    var result = "";

                    result += $"<span class=\"badge badge--s badge--empty badge--bold\">{Items.ToList().Count}</span>";
                    foreach (var item in Items)
                    {
                        result += " " + string.Format(ResHelper.GetString("Kadena.OrdersList.ItemsFormatString"), item.Quantity, item.Name);
                    }
                    return result.Substring(0, result.Length - 1);
                }
                return "<span class=\"badge badge--s badge--empty badge--bold\">0</span>";
            }
        }
    }
}