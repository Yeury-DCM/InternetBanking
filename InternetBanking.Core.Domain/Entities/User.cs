using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class User
    {
        public required int Id { get; set; }
        public required string UserName { get; set; }
        public required string Name { get; set; }
        public required string  LastName { get; set; }
        public required string Identification  { get; set; }
        public required string Mail { get; set; }
        public required string Password { get; set; }
        public required int UserTypeID { get; set; }
        public required bool Status { get; set; }

        //Navegation Properties

        public UserType userType { get; set; }
        public ICollection<Product> products { get; set; } = new List<Product>();
        public ICollection<Beneficiary> beneficiaries { get; set; } = new List<Beneficiary>();
        public ICollection<Transaction> transactions { get; set; } = new List<Transaction>();

    }
}
