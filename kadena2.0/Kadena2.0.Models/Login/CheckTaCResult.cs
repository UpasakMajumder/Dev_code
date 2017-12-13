namespace Kadena.Models.Login
{
    public class CheckTaCResult
    {
        public bool LogonSuccess { get; set; }
        public bool ShowTaC { get; set; }
        public string Url { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorPropertyName { get; set; }

        public static CheckTaCResult GetFailedResult(string field, string message)
        {
            return new CheckTaCResult
            {
                LogonSuccess = false,
                ShowTaC = false,
                Url = string.Empty,
                ErrorPropertyName = field,
                ErrorMessage = message
            };
        }

    }
}
