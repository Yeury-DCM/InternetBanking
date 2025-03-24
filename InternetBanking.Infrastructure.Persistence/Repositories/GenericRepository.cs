using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : class
    {
        private readonly ApplicationContext _dbcontext;

        public GenericRepository(ApplicationContext context) { _dbcontext = context; }
        public async Task<Entity> AddAsync(Entity entity)
        {
           await _dbcontext.Set<Entity>().AddAsync(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Entity entity)
        {
            _dbcontext.Set<Entity>().Remove(entity);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<List<Entity>> GetAllAsync()
        {
            return await _dbcontext.Set<Entity>().ToListAsync();
           
        }

        public async Task<List<Entity>> GetAllWithIncludesAsync(List<string> properties)
        {
            var query = _dbcontext.Set<Entity>().AsQueryable();
            if (properties != null)
            {
                foreach (string property in properties)
                {
                    query = query.Include(property);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<Entity> GetByIdAsync(int id)
        {
            return await _dbcontext.Set<Entity>().FindAsync(id);
        }

        public async Task UpdateAsync(Entity entity, int id)
        {
            Entity entry = await _dbcontext.Set<Entity>().FindAsync(id);
            _dbcontext.Entry(entry).CurrentValues.SetValues(entity);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
