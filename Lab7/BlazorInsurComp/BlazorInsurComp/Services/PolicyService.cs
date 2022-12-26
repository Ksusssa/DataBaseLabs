using lab6.WebApplication.Contexts;
using Lab6.WebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorInsurComp.Services
{
    public class PolicyService
    {
        private InsuranceCompanyDbContext _context;

        public PolicyService(InsuranceCompanyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Policy>?> GetAll()
        {
            return await Task.FromResult(_context.Policies.Include(p => p.PolicyType).Include(c => c.Client).Include(e => e.Employee).ToList());
        }

        public Policy? Delete(int id)
        {
            Policy? policy = _context.Policies.First(p => p.Id == id);
            _context.Policies.Remove(policy);
            _context.SaveChanges();
            return policy;
        }

        public Policy? Get(int id)
        {
            Policy? policy = _context.Policies.FirstOrDefault(p => p.Id == id);
            return policy;
        }

        public bool Update(Policy? policy)
        {
            if (policy != null)
            {
                _context.Policies.Update(policy);
                _context.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public int Create(Policy? policy)
        {
            if (policy != null)
            {
                _context.Policies.Add(policy);
                _context.SaveChanges();
                return Convert.ToInt32(policy.Id);
            }
            else return -1;
        }
    }
}
