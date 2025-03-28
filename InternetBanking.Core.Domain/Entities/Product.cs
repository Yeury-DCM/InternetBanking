using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public required string ProductNumber { get; set; }
        public required int ProductTypeID { get; set; }
        public decimal Balance { get; set; }
        public  decimal? Limit { get; set; }
        public  bool? IsPrincipal { get; set; }
        public required string UserID { get; set; }

        //Navegation Properties
        public ProductType productType { get; set; }
        public ICollection<Transaction> transactions { get; set; } = new List<Transaction>();
    }
  

     
}
