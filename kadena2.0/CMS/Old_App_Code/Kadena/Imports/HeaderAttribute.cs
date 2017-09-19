using System;

namespace Kadena.Old_App_Code.Kadena.Imports
{
    public class HeaderAttribute : Attribute
    {
        public int Order { get; private set; }
        public string Title { get; private set; }

        public HeaderAttribute(int order, string title)
        {
            Order = order;
            Title = title;
        }
    }
}