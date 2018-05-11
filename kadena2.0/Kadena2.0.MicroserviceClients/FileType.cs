namespace Kadena2.MicroserviceClients
{
    public class FileType
    {
        private readonly string _value;

        public static FileType Original { get; } = new FileType("original");

        private FileType(string value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
