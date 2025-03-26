using Microsoft.AspNetCore.Mvc;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.DasboardVMS;

namespace InternetBanking.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dasboardService;

        public DashboardController(IDashboardService dasboardService)
        {
            _dasboardService = dasboardService;
        }
        public async Task<IActionResult> Index()
        {
            DashboardViewModel vm = await _dasboardService.DashboardInfo();
            return View(vm);
        }
    }
}
