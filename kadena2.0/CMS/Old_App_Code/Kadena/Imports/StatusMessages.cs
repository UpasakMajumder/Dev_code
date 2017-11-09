using System.Collections;
using System.Collections.Generic;

namespace Kadena.Old_App_Code.Kadena.Imports
{
    /// <summary>
    /// Stores first N (parameter <paramref name="Capacity"/>) status messages
    /// </summary>
    public class StatusMessages : IEnumerable<string>
    {
        private List<string> list = new List<string>();
        public int Capacity { get; private set; }
        public int AllMessagesCount { get; private set; }

        public StatusMessages(int capacity)
        {
            this.Capacity = capacity;
        }

        public void Add(string message)
        {
            AllMessagesCount++;

            if (list.Count < Capacity)
            {
                list.Add(message);
            }
        }

        public void Clear()
        {
            list.Clear();
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