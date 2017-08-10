namespace Kadena2.MicroserviceClients
{
    public class FileFolder
    {
        private string _value;

        public static FileFolder OriginalMailing { get; } = new FileFolder { _value = "original-mailing" };
        public static FileFolder Artworks { get; } = new FileFolder { _value = "artworks" };

        public override string ToString()
        {
            return _value;
        }
    }
}
