namespace Kadena2.MicroserviceClients
{
    public class FileFolder
    {
        private readonly string _value;

        public static FileFolder OriginalMailing { get; } = new FileFolder("original-mailing");
        public static FileFolder Artworks { get; } = new FileFolder("artworks");        

        private FileFolder(string value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
