﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepository<Entity> where Entity : class
    {

        Task<Entity> AddAsync(Entity entity);
        Task<Entity> GetByIdAsync(int id);
        Task<List<Entity>> GetAllAsync();
        Task<List<Entity>> GetAllWithIncludesAsync(List<String> properties);
        Task UpdateAsync(Entity entity, int id);
        Task DeleteAsync(Entity entity);
    }
}
