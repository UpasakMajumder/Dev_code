using System.Reflection;

namespace Kadena.Old_App_Code.Kadena.Imports
{
    public class Column
    {
        public PropertyInfo PropertyInfo { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsMandatory { get; set; }
    }
}