namespace Kadena.Dto.Logon.Responses
{
    public class CheckTaCResultDTO
    {
        public string ErrorMessage { get; set; }
        public string ErrorPropertyName { get; set; }
        public bool ShowTaC { get; set; }
        public string Url { get; set; }
    }
}
