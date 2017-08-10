namespace Kadena2.MicroserviceClients
{
    public class FileModule
    {
        private string _value;

        public static FileModule KList { get; } = new FileModule { _value = "KList" };
        public static FileModule KProducts { get; } = new FileModule { _value = "KProducts" };

        public override string ToString()
        {
            return _value;
        }
    }
}
