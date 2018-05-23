using Kadena.Models.RecentOrders;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IOrderListService
    {
        string PageCapacityKey { get; set; }

        bool EnablePaging { get; set; }

        Task<OrderHead> GetOrdersToApprove();

        Task<OrderHead> GetHeaders();

        Task<OrderBody> GetBody(int pageNumber);

        Task<OrderHeadBlock> GetCampaignHeaders(string orderType, int campaignID);
    }
}
