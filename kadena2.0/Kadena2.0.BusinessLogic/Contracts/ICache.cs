using System;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ICache
    {
        void Insert(string key, object value);
        void Insert(string key, object value, DateTime expiration);
        object Get(string key);
    }
}
