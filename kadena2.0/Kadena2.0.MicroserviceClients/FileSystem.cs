using System.Collections.Generic;
using System.Linq;

namespace Kadena2.MicroserviceClients
{
    public class FileSystem
    {
        private readonly string _value;

        public static FileSystem Mailing { get; } = new FileSystem("mailing");

        private static readonly Dictionary<FileSystem, string> systemPaths = new Dictionary<FileSystem, string>
        {
            { Mailing, "klist/"}
        };

        public static FileSystem Create(string path)
        {
            return systemPaths.FirstOrDefault(sp => sp.Value.Equals(path)).Key;
        }

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