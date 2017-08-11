namespace Kadena2.MicroserviceClients
{
    public class FileModule
    {
        private readonly string _value;

        public static FileModule KList { get; } = new FileModule("KList");
        public static FileModule KProducts { get; } = new FileModule("KProducts");

        private FileModule(string value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
