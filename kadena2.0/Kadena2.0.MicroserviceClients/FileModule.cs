using System;
using System.Linq;
using System.Reflection;

namespace Kadena2.MicroserviceClients
{
    public class FileModule
    {
        private readonly string _value;

        public static FileModule KList { get; } = new FileModule("KList");
        public static FileModule KProducts { get; } = new FileModule("KProducts");
        public static FileModule KDesign { get; } = new FileModule("kdesign");

        private FileModule(string value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value;
        }
        public static implicit operator FileModule(string source)
        {
            var type = typeof(FileModule);
            var modulePropertyMatching = type.GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == type)
                .FirstOrDefault(p => (p.GetValue(null,null) as FileModule)._value.Equals( source, StringComparison.InvariantCultureIgnoreCase));

            if (modulePropertyMatching != null)
            {
                return modulePropertyMatching.GetValue(null, null) as FileModule;
            }

            throw new InvalidCastException($"Invalid file module string '{source}'");
        }
    }
}