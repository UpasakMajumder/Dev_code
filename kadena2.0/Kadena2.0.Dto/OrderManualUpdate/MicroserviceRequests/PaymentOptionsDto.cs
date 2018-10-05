namespace Kadena.Dto.OrderManualUpdate.MicroserviceRequests
{
    public class PaymentOptionsDto
    {
        public string PaymentOptionName { get; set; }

        /// <summary>
        /// This is the customer reference field (Commenly their PO Number)
        /// </summary>
        public string PONumber { get; set; }

        public string TransactionKey { get; set; }

        public string PaymentGatewayName { get; set; }

        public string PaymentGatewayCustomerCode { get; set; }

        public string TokenId { get; set; }
    }
}
