using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.DasboardVMS
{
    public class DashboardViewModel
    {
        public int productsCount { get; set; }
        public int transactionsCount { get; set; }
        public int paymentsCount { get; set; }
        public int todayTransactionsCount { get; set; }
        public int todayPaymentsCount { get; set; }

        public List<Product> products { get; set; } = new List<Product>();
        public List<Transaction> transactions { get; set; } = new List<Transaction>();
        //public int ClientesActivos { get; set; }
        //public int ClientesInactivos { get; set; }
    }
}
