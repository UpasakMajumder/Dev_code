namespace Kadena.Models.Login
{
    public class CheckTaCResult
    {
        public bool LogonSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorPropertyName { get; set; }
        public string ShowTaC { get; set; }
        public string Url { get; set; }
    }
}
