using CMS.CustomTables;
using CMS.Ecommerce;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Linq;

namespace Kadena2.WebAPI.KenticoProviders.Providers
{
    public class KenticoOrderProvider : IKenticoOrderProvider
    {
        private readonly IKenticoResourceService resource;

        public KenticoOrderProvider(IKenticoResourceService resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            this.resource = resource;
        }

        public string MapOrderStatus(string microserviceStatus)
        {
            var genericStatusItem = CustomTableItemProvider.GetItems("KDA.OrderStatusMapping")
                .FirstOrDefault(i => i["MicroserviceStatus"].ToString().ToLower() == microserviceStatus.ToLower());

            var resourceKey = genericStatusItem?.GetValue("GenericStatus")?.ToString();

            return string.IsNullOrEmpty(resourceKey) ? microserviceStatus : resource.GetResourceString(resourceKey);
        }

        public int GetOrderStatusId(string name)
        {
            var status = OrderStatusInfoProvider.GetOrderStatuses().Where(s => s.StatusName == name).FirstOrDefault();
            return status?.StatusID ?? 0;
        }
    }
}
