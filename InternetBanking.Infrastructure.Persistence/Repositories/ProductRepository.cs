using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class ProductRepository: GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationContext _dbcontext;

        public ProductRepository(ApplicationContext applicationContext): base(applicationContext) 
        {
            _dbcontext = applicationContext;
        }
    }
}
