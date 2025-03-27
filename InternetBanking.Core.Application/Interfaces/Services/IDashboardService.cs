using InternetBanking.Core.Application.ViewModels.DasboardVMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> DashboardInfo();
        Task<DashboardViewModel> GetUserProductsInfo();
    }
}
