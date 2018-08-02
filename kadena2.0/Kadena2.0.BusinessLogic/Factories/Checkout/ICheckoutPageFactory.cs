using Kadena.Models;
using Kadena.Models.Checkout;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Factories.Checkout
{
    public interface ICheckoutPageFactory
    {
        CartEmptyInfo CreateCartEmptyInfo();
        CartItems CreateProducts(List<CheckoutCartItem> cartItems, ShoppingCartTotals cartItemsTotals, string countOfItemsString);
        CartPrice CreateCartPrice(ShoppingCartTotals cartItemsTotals);
        DeliveryAddresses CreateDeliveryAddresses(List<DeliveryAddress> addresses, string userNotificationString, bool otherAddressAvailable);
        PaymentMethods CreatePaymentMethods(PaymentMethod[] paymentMethods);
        SubmitButton CreateSubmitButton();
        DeliveryDate CreateDeliveryDateInput();
        NotificationEmail CreateNotificationEmail(bool emailConfirmationEnabled);        
    }
}