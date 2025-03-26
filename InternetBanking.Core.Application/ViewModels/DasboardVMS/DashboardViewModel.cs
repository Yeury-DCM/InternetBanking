using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.DasboardVMS
{
    public class DashboardViewModel
    {
        public int products { get; set; }
        public int transactions { get; set; }
        public int payments { get; set; }
        public int todayTransactions { get; set; }
        public int todayPayments { get; set; }
        //public int ClientesActivos { get; set; }
        //public int ClientesInactivos { get; set; }
    }
}
