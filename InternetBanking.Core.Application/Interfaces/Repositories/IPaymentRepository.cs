using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
   public interface IPaymentRepository 
    {
        Task<Product> Update(Product Entity, int id);
        Task<Product> GetByIdWithIncludesAsync(int id, List<string> properties);
        Task<Product> GetByNumberWithIncludesAsync(string PNumber, List<string> properties);
    }
}
