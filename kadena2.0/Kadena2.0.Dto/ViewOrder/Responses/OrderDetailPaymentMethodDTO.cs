using Kadena.Dto.Checkout;

namespace Kadena.Dto.ViewOrder.Responses
{

    public class OrderDetailPaymentMethodDTO
    {
        public PaymentMethodsDTO Ui { get; set; }
        public PaymentMethodSelectedDTO CheckedObj { get; set; }
    }
}
