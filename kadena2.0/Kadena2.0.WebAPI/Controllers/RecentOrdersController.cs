using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Factories;
using Kadena.Dto.RecentOrders;
using Kadena.Dto.ViewOrder.Responses;
using Kadena.Helpers.Routes;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class RecentOrdersController : ApiControllerBase
    {
        private readonly IOrderDetailService orderDetailService;
        private readonly IMapper _mapper;
        private readonly IOrderListService _orderService;
        private readonly IOrderHistoryService _orderHistoryService;

        public RecentOrdersController(
            IOrderDetailService orderDetailService, 
            IOrderListServiceFactory orderListServiceFactory,
            IOrderHistoryService orderHistoryService,
            IMapper mapper)
        {
            _orderHistoryService = orderHistoryService ?? throw new ArgumentNullException(nameof(orderHistoryService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.orderDetailService = orderDetailService ?? throw new ArgumentNullException(nameof(orderDetailService));

            if (orderListServiceFactory == null)
            {
                throw new ArgumentNullException(nameof(orderListServiceFactory));
            }
            _orderService = orderListServiceFactory.GetRecentOrders();
        }

        [HttpGet]
        [Route("api/recentorders/getbody/{pageNumber}")]
        public async Task<IHttpActionResult> GetBody(int pageNumber)
        {
            var orderBody = await _orderService.GetBody(pageNumber);
            var result = _mapper.Map<OrderBodyDto>(orderBody);
            return ResponseJson(result);
        }

        [HttpGet]
        [Route(Routes.Order.GetToApprove)]
        public async Task<IHttpActionResult> GetOrdersToApprove()
        {
            var orders = await _orderService.GetOrdersToApprove();
            var result = _mapper.Map<OrderHeadDto>(orders);
            return ResponseJson(result);
        }

        [HttpGet]
        [Route(Routes.Order.GetCampaignOrdersToApprove)]
        public async Task<IHttpActionResult> GetCampaignOrdersToApprove(string orderType)
        {
            var orders = await _orderService.GetCampaignOrdersToApprove(orderType, 0);
            var result = _mapper.Map<OrderHeadBlockDto>(orders);
            return ResponseJson(result);
        }

        [HttpGet]
        [Route("api/recentorders/getheaders")]
        public async Task<IHttpActionResult> GetHeaders()
        {
            var orderHead = await _orderService.GetHeaders();
            var result = _mapper.Map<OrderHeadDto>(orderHead);
            return ResponseJson(result);
        }

        [HttpGet]
        [Route("api/recentorders/getheaders/{orderType}")]
        public async Task<IHttpActionResult> GetHeaders(string orderType)
        {
            var orderHead = await _orderService.GetCampaignHeaders(orderType, 0);
            var result = _mapper.Map<OrderHeadBlockDto>(orderHead);
            return ResponseJson(result);
        }

        [HttpGet]
        [Route("api/recentorders/getheaders/{orderType}/{campaignID}")]
        public async Task<IHttpActionResult> GetHeaders(string orderType, int campaignID)
        {
            var orderHead = await _orderService.GetCampaignHeaders(orderType, campaignID);
            var result = _mapper.Map<OrderHeadBlockDto>(orderHead);
            return ResponseJson(result);
        }

        [HttpGet]
        [Route(Routes.Order.Detail)]
        public async Task<IHttpActionResult> Get(string orderId)
        {
            var detailPage = await orderDetailService.GetOrderDetail(orderId);
            try
            {
                var detailPageDto = _mapper.Map<OrderDetailDTO>(detailPage);
                return ResponseJson(detailPageDto);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet]
        [Route(Routes.Order.History)]
        public async Task<IHttpActionResult> GetHistory(string orderId)
        {
            var history = await _orderHistoryService.GetOrderHistory(orderId);
            var historyDto = _mapper.Map<OrderHistoryDto>(history);
            return ResponseJson(historyDto);
        }
    }
}