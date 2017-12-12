namespace Kadena.Dto.Logon.Responses
{
    public class LogonUserResultDTO
    {
        public bool LogonSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorPropertyName { get; set; }
    }
}
