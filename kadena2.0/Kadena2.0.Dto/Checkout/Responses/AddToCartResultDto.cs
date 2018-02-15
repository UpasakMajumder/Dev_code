namespace Kadena.Dto.Checkout.Responses
{
    public class AddToCartResultDto
    {
        public CartItemsPreviewDTO CartPreview { get; set; }

        public RequestResultDto Confirmation { get; set; }
    }
}
