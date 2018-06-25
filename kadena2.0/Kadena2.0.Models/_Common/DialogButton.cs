namespace Kadena.Models.Common
{
    public class DialogButton<T>
    {
        public string Button { get; set; }
        public string ProceedUrl { get; set; }
        public T Dialog { get; set; }
    }
}
