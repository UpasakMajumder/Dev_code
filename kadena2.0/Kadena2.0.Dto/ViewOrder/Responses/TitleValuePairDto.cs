namespace Kadena.Dto.ViewOrder.Responses
{
    public class TitleValuePairDto<T>
    {
        public string Title { get; set; }
        public T Value { get; set; }
    }
}
