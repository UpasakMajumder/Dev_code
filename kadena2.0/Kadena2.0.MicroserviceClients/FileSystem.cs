using System.Collections.Generic;
using System.Linq;

namespace Kadena2.MicroserviceClients
{
    public class FileSystem
    {
        private readonly string _value;
        public string SystemFolder { get; }

        public static FileSystem Mailing { get; } = new FileSystem("mailing", "klist/");

        private static readonly Dictionary<FileSystem, string> systemPaths = new Dictionary<FileSystem, string>
        {
            { Mailing, Mailing.SystemFolder}
        };

        public static FileSystem Create(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            return systemPaths.FirstOrDefault(sp => path.StartsWith(sp.Value)).Key;
        }

        private FileSystem(string value, string systemFolder)
        {
            _value = value;
            SystemFolder = systemFolder;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}