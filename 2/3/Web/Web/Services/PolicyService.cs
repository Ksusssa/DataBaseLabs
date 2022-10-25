using InsuranceLib.Data;
using InsuranceLib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Web.Services
{
    public class PolicyService : ITableService<Policy>
    {
        public InsuranceContext Context { get; } 

        public IMemoryCache Cache { get; }

        public int CacheTime { get; }

        public PolicyService(InsuranceContext context, IMemoryCache cache)
        {
            Context = context;
            Cache = cache;
            CacheTime = 240;
        }

        public IEnumerable<Policy> GetAll()
        {
            return Context.Policies
                .Include(x => x.Client)
                .Include(x => x.Employee)
                .Include(x => x.TypesOfPolicie).AsEnumerable();
        }

        public IEnumerable<Policy> GetByCondition(Func<Policy, bool> predicate)
        {
            IEnumerable<Policy> policies = null;
            if (!Cache.TryGetValue(predicate.GetHashCode(),out policies))
            {
                policies = GetAll().Where(x => predicate(x));
                if (policies != null)
                {
                    Cache.Set(predicate.GetHashCode(), policies, TimeSpan.FromSeconds(CacheTime));
                }
            }
            return policies;
        }

        public IEnumerable<Policy> GetFromCach(int count, string cachKey)
        {
            IEnumerable<Policy> policies;
            if (!Cache.TryGetValue(cachKey,out policies))
            {
                policies = GetAll().Take(count);
                if (policies != null)
                {
                    Cache.Set(cachKey, policies,TimeSpan.FromSeconds(CacheTime));
                }
            }
            return policies;
        }
    }
}
