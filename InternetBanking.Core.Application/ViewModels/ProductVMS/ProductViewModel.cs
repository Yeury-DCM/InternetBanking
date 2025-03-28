using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.ProductVMS
{
    public class ProductViewModel
    {
        public required int Id { get; set; }
        public required string ProductNumber { get; set; }
        public required int ProductTypeID { get; set; }
        public required decimal Balance { get; set; }
        public decimal Limit { get; set; }
        public bool IsPrincipal { get; set; }
        public required string UserID { get; set; }

        //Navegation Properties
        public ProductType productType { get; set; }
        public ICollection<Transaction> transactions { get; set; } = new List<Transaction>();
    }
}
