using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using CMS.Helpers;

namespace Kadena.Old_App_Code.Kadena.Orders
{
    [DataContract]
    public class OrderHistoryData
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public DateTime createDate { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public DateTime deliveryDate { get; set; }
        [DataMember]
        public IEnumerable<OrderItemHistoryData> items { get; set; }

        [IgnoreDataMember]
        public string ItemsString
        {
            get
            {
                if (items != null && items.ToList().Count > 0)
                {
                    var result = "";

                    result += $"<span class=\"badge badge--s badge--empty badge--bold\">{items.ToList().Count}</span>";
                    foreach (var item in items)
                    {
                        result += " " + string.Format(ResHelper.GetString("Kadena.OrdersList.ItemsFormatString"), item.quantity, item.name);
                    }
                    return result.Substring(0, result.Length - 1);
                }
                return "<span class=\"badge badge--s badge--empty badge--bold\">0</span>";
            }
        }
    }
}