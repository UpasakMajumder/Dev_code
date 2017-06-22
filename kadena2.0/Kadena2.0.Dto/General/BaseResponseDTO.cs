namespace Kadena.Dto.General
{
    public class BaseResponseDTO<T>
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public T Payload { get;set;}

        public string ErrorMessages
        {
            get
            {
                return Error.Message;
            }
            set
            {
                Error = new ErrorMessageDTO { Message = value };
            }
        }

        public ErrorMessageDTO Error { get; set; }
    }
}
