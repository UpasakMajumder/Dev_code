using Kadena.Dto.Checkout;

namespace Kadena.Dto.Product
{
    public class AddToCartResultDto
    {
        public CartItemsPreviewDTO CartPreview { get; set; }

        public RequestResultDto Confirmation { get; set; }
    }
}
