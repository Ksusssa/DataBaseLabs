using InsuranceLib.Data;
using InsuranceLib.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Web.Services
{
    public class EmployeeService : ITableService<Employee>
    {
        public InsuranceContext Context { get; }

        public IMemoryCache Cache { get; }

        public int CacheTime { get; }

        public EmployeeService(InsuranceContext context, IMemoryCache cache)
        {
            Context = context;
            Cache = cache;
            CacheTime = 240;
        }

        public IEnumerable<Employee> GetAll()
        {
            return Context.Employees.AsEnumerable();
        }

        public IEnumerable<Employee> GetByCondition(Func<Employee, bool> predicate)
        {
            IEnumerable<Employee> employees = null;
            if (!Cache.TryGetValue(predicate.GetHashCode(), out employees))
            {
                employees = GetAll().Where(x => predicate(x));
                if (employees != null)
                {
                    Cache.Set(predicate.GetHashCode(), employees, TimeSpan.FromSeconds(CacheTime));
                }
            }
            return employees;
        }

        public IEnumerable<Employee> GetFromCach(int count, string cachKey)
        {
            IEnumerable<Employee> employees;
            if (!Cache.TryGetValue(cachKey, out employees))
            {
                employees = GetAll().Take(count);
                if (employees!= null)
                {
                    Cache.Set(cachKey, employees, TimeSpan.FromSeconds(CacheTime));
                }
            }
            return employees;
        }
    }
}
