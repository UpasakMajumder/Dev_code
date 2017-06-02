namespace Kadena.Dto.SubmitOrder
{
    public class PaymentOptionDTO
    {
        public int KenticoPaymentOptionID { get; set; }
        public string PaymentOptionName { get; set; }

        /// <summary>
        /// This is the customer reference field (Commenly their PO Number)
        /// </summary>
        public string PONumber { get; set; }
    }
}
