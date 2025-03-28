using InternetBanking.Core.Application.Services;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace InternetBanking.Core.Application.ViewModels.PayementVMS
{
   public  class PaymentViewModel
    {

        public  int Id { get; set; }
        public  string DestinationProductNumber { get; set; }
        public  Product OriginProduct { get; set; }
        public  int ProductTypeID { get; set; }
        public  string UserID { get; set; }
        public  decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }

        //Navegation Properties
        public User User { get; set; }
        public ProductType productType { get; set; }
    }
}
