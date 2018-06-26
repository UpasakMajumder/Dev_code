namespace Kadena.Dto.Common
{
    public class DialogButtonDTO<T>
    {
        public string Button { get; set; }
        public string ProceedUrl { get; set; }
        public T Dialog { get; set; }
    }
}
