using lab6.WebApplication.Contexts;
using Lab6.WebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorInsurComp.Services
{
    public class PolicyTypeService
    {
        private InsuranceCompanyDbContext _context;
        public PolicyTypeService(InsuranceCompanyDbContext context)
        {
            _context = context;
        }

        public async Task<List<PolicyType>> GetAll()
        {
            return await Task.FromResult(_context.TypeOfPolicies.Include(r => r.Risks).ToList());
        }

        public PolicyType? Delete(int id)
        {
            PolicyType? policyType = _context.TypeOfPolicies.First(p => p.Id == id);
            _context.TypeOfPolicies.Remove(policyType);
            _context.SaveChanges();
            return policyType;
        }

        public PolicyType? Get(int id)
        {
            PolicyType? policyType = _context.TypeOfPolicies.FirstOrDefault(p => p.Id == id);
            return policyType;
        }

        public bool Update(PolicyType? policyType)
        {
            if (policyType != null)
            {
                _context.TypeOfPolicies.Update(policyType);
                _context.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public bool Create(PolicyType? policyType)
        {
            if (policyType != null)
            {
                _context.TypeOfPolicies.Add(policyType);
                _context.SaveChanges();
                return true;
            }
            else
                return false;
        }
    }
}
