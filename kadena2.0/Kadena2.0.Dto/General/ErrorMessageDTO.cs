namespace Kadena.Dto.General
{
    public class ErrorMessageDTO
    {
        public string Message { get; set; }
        public ErrorMessageDTO InnerError { get; set; }
    }
}