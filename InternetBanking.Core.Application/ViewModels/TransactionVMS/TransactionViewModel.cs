using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.TransactionVMS
{
     public class TransactionViewModel
    {

        public required int Id { get; set; }
        public required int UserID { get; set; }
        public required int ProductID { get; set; }
        public required int TransactionTypeID { get; set; }
        public required decimal Amount { get; set; }
        public required DateTime TransactionDate { get; set; }

        //Navegation Properties

        public User User { get; set; }
        public Product Product { get; set; }
        public TransactionType transactionType { get; set; }
    }
}
