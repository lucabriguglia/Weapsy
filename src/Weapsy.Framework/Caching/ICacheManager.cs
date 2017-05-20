using System.Threading.Tasks;

namespace Weapsy.Framework.Caching
{
	public interface ICacheManager
	{
		T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        void Set(string key, object data, int cacheTime);
		bool IsSet(string key);
		void Remove(string key);
        Task RemoveAsync(string key);
        void RemoveByPattern(string pattern);
		void Clear();
	}
}