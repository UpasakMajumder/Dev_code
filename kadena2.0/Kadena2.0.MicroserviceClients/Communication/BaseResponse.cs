namespace Kadena2.MicroserviceClients.Communication
{
    public class BaseResponse<T> 
    {
        public bool Success { get; set; }

        public T Payload { get; set; }

        public string ErrorMessage { get; set; }
    }
}