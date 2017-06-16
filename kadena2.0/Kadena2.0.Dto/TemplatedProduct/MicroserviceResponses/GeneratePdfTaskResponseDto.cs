namespace Kadena.Dto.TemplatedProduct.MicroserviceResponses
{
    public class GeneratePdfTaskResponseDto
    {
        public string TaskId { get; set; }
        public bool Started { get; set; }
        public bool Finished { get; set; }
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
        public object FileName { get; set; }
    }
}
