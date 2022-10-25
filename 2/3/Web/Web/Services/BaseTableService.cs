using InsuranceLib.Data;
using Microsoft.Extensions.Caching.Memory;

namespace Web.Services
{
    public interface ITableService<T>
    {
        public InsuranceContext Context { get; }
        public IMemoryCache Cache { get; }

        public int CacheTime { get; }
        public IEnumerable<T> GetAll();
        public IEnumerable<T> GetByCondition(Func<T, bool> predicate);
        public IEnumerable<T> GetFromCach(int count, string cachKey);
    }
}
