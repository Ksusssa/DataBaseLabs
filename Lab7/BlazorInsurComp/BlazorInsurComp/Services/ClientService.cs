using lab6.WebApplication.Contexts;
using Lab6.WebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorInsurComp.Services
{
    public class ClientService
    {
        private InsuranceCompanyDbContext _context;
        public ClientService(InsuranceCompanyDbContext context)
        {
            _context = context;
        }

        public async Task<List <Client>> GetAll()
        {
            return await Task.FromResult(_context.Clients.Include(n=> n.User).ToList());
        }

        public Client? Delete(int id)
        {
            Client? client = _context.Clients.First(c => c.Id == id);
            _context.Clients.Remove(client);
            _context.SaveChanges();
            return client;
        }

        public Client? Get(int id)
        {
            Client? client = _context.Clients.FirstOrDefault(c => c.Id == id);
            return client;
        }

        public bool Update(Client? client)
        {
            if (client != null)
            {
                _context.Clients.Update(client);
                _context.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public bool Create(Client? client)
        {
            if (client != null)
            {
                _context.Clients.Add(client);
                _context.SaveChanges();
                return true;
            }
            else
                return false;
        }
    }
}
