namespace Kadena.Models.Login
{
    public class CheckTaCResult
    {
        public string ErrorMessage { get; set; }
        public string ErrorPropertyName { get; set; }
        public bool ShowTaC { get; set; }
        public string Url { get; set; }
    }
}
