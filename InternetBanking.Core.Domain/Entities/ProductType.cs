using InternetBanking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class ProductType: BaseBasicTypeEntity
    {
 
        public override int Id {  get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
