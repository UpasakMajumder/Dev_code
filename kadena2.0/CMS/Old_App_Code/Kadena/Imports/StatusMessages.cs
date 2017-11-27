using System.Collections;
using System.Collections.Generic;

namespace Kadena.Old_App_Code.Kadena.Imports
{
    /// <summary>
    /// Stores first N (parameter <paramref name="Capacity"/>) status messages
    /// </summary>
    public sealed class StatusMessages : IEnumerable<string>
    {
        private List<string> list;

        public int Capacity
        {
            get
            {
                return list.Capacity;
            }
        }

        public int AllMessagesCount { get; private set; }

        public StatusMessages(int capacity)
        {
            list = new List<string>(capacity);
        }

        public void Add(string message)
        {
            AllMessagesCount++;

            if (list.Count < list.Capacity)
            {
                list.Add(message);
            }
        }

        public void Clear()
        {
            list.Clear();
            AllMessagesCount = 0;
        }

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}