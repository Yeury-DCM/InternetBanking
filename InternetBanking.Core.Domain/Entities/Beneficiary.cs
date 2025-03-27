using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class Beneficiary
    {
        public required int Id { get; set; }
        public required int UserID { get; set; }
        public required int ProductID { get; set; }

        //Navegation properties
        public User user { get; set; }
        public Product account { get; set; }

    }

}
