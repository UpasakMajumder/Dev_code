using Kadena.Models;
using Kadena.Models.Checkout;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Factories.Checkout
{
    public interface ICheckoutPageFactory
    {
        CartEmptyInfo CreateCartEmptyInfo();
        CartItems CreateProducts(List<CartItem> cartItems, ShoppingCartTotals cartItemsTotals, string countOfItemsString);
        CartPrice CreateCartPrice(ShoppingCartTotals cartItemsTotals);
        DeliveryAddresses CreateDeliveryAddresses(List<DeliveryAddress> addresses, string userNotificationString, bool otherAddressAvailable);
        AddressDialog GetOtherAddressDialog();
        PaymentMethods CreatePaymentMethods(PaymentMethod[] paymentMethods);
        SubmitButton CreateSubmitButton();
        NotificationEmail CreateNotificationEmail(bool emailConfirmationEnabled);        
    }
}