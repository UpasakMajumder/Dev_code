namespace Kadena.Dto.Search.Responses
{
    public class AutocompleteResponseDTO
    {
        public AutocompleteProductsDTO Products { get; set; }
        public AutocomletePagesDTO Pages { get; set; }

        public string Message { get; set; }
    }
}
