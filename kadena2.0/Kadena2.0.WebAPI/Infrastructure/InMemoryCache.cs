using Kadena.WebAPI.Contracts;
using System;
using System.Web;
using System.Web.Caching;

namespace Kadena.WebAPI.Infrastructure
{
    public class InMemoryCache : ICache
    {
        public virtual Cache Cache => HttpContext.Current.Cache;

        public object Get(string key)
        {
            return Cache.Get(key);
        }

        public void Insert(string key, object value)
        {
            Cache.Insert(key, value);
        }

        public void Insert(string key, object value, DateTime expiration)
        {
            Cache.Insert(key, value, dependencies: null, absoluteExpiration: expiration, slidingExpiration: Cache.NoSlidingExpiration);
        }
    }
}