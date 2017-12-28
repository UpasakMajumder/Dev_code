using Kadena.BusinessLogic.Contracts;
using System;
using System.Web.Caching;

namespace Kadena.WebAPI.Infrastructure
{
    public class InMemoryCache : ICache
    {
        private Cache cache;
        public Cache Cache
        {
            get
            {
                if (cache == null)
                {
                    TryInitCache();
                }

                return cache;
            }
        }

        public InMemoryCache()
        {
            TryInitCache();
        }

        private void TryInitCache()
        {
            cache = System.Web.HttpContext.Current?.Cache;
        }

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