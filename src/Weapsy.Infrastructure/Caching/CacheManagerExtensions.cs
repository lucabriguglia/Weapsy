using System;

namespace Weapsy.Infrastructure.Caching
{
	public static class CacheManagerExtensions
	{
		public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
		{
			return Get(cacheManager, key, 60, acquire);
		}

		public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
		{
			if (cacheTime <= 0)
				return acquire();

            var data = cacheManager.Get<T>(key);

            if (data != null)
                return data;

			var result = acquire();

			cacheManager.Set(key, result, cacheTime);

			return result;
		}
	}
}