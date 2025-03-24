using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationContext _dbcontext;
        public PaymentRepository(ApplicationContext context) { _dbcontext = context; }

        public async Task<Product> GetByIdAsync(int Id)
        {
            return await _dbcontext.Set<Product>().FindAsync(Id);
        }

        public async Task<Product> GetByProductNumberAsync(string productNumber)
        {
            return await _dbcontext.Set<Product>().FirstOrDefaultAsync(p => p.ProductNumber == productNumber);

        }
        public async Task<Product> Update(Product Entity, int id)
        {
            Product entry = await _dbcontext.Set<Product>().FindAsync(id);
            _dbcontext.Entry(entry).CurrentValues.SetValues(Entity);
            await _dbcontext.SaveChangesAsync();
            return entry;

        }

        public async Task<Product> GetByIdWithIncludesAsync(int id, List<string> properties)
        {
            var query = _dbcontext.Set<Product>().AsQueryable();
            if (properties != null)
            {
                foreach (string property in properties)
                {
                    query = query.Include(property);
                }
            }
            return await query.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> GetByNumberWithIncludesAsync(string PNumber, List<string> properties)
        {
            var query = _dbcontext.Set<Product>().AsQueryable();
            if (properties != null)
            {
                foreach (string property in properties)
                {
                    query = query.Include(property);
                }
            }
            return await query.FirstOrDefaultAsync(p => p.ProductNumber == PNumber);
        }
    }
}
