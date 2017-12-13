namespace Kadena.Dto.Logon.Responses
{
    public class CheckTaCResultDTO
    {
        public bool LogonSuccess { get; set; }
        public bool ShowTaC { get; set; }
        public string Url { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorPropertyName { get; set; }
    }
}
