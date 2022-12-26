using lab6.WebApplication.Contexts;
using Lab6.WebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorInsurComp.Services
{
    public class RiskService
    {
        private InsuranceCompanyDbContext _context;
        public RiskService(InsuranceCompanyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Risk>> GetAll()
        {
            return await Task.FromResult(_context.Risks.Include(r => r.TypesOfPolicies).ToList());
        }

        public Risk? Delete(int id)
        {
            Risk? risk = _context.Risks.First(p => p.Id == id);
            _context.Risks.Remove(risk);
            _context.SaveChanges();
            return risk;
        }

        public Risk? Get(int id)
        {
            Risk? risk = _context.Risks.FirstOrDefault(p => p.Id == id);
            return risk;
        }

        public bool Update(Risk? risk)
        {
            if (risk != null)
            {
                _context.Risks.Update(risk);
                _context.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public bool Create(Risk? risk)
        {
            if (risk != null)
            {
                _context.Risks.Add(risk);
                _context.SaveChanges();
                return true;
            }
            else
                return false;
        }
    }
}