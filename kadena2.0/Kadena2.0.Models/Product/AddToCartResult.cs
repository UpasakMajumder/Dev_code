using Kadena.Models.Checkout;

namespace Kadena.Models.Product
{
    public class AddToCartResult
    {
        public CartItemsPreview CartPreview { get; set; }

        public RequestResult Confirmation { get; set; }
    }
}
