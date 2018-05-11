namespace Kadena2.MicroserviceClients
{
    public class FileSystem
    {
        private readonly string _value;

        public static FileSystem Mailing { get; } = new FileSystem("mailing");

        private FileSystem(string value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}