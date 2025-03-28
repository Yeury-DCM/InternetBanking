using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Domain.Entities
{
    public class Transaction
    {
        public required int Id { get; set; }
        public required string UserID { get; set; }
        public required int ProductID { get; set; }
        public required int TransactionTypeID { get; set; }
        public required decimal Amount { get; set; }
        public required DateTime TransactionDate { get; set; }

        //Navegation Properties
        public Product Product { get; set; }   
        public TransactionType transactionType { get; set; }
    }
   
}
