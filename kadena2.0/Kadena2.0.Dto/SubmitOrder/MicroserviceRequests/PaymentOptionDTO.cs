namespace Kadena.Dto.SubmitOrder.MicroserviceRequests
{
    public class PaymentOptionDTO
    {
        public string PaymentOptionName { get; set; }
        public string PONumber { get; set; }
        public string TransactionKey { get; set; }
        public string PaymentGatewayCustomerCode { get; set; }
        public string TokenId { get; set; }
    }
}
