using InternetBanking.Core.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Web.Controllers
{
    
    [Authorize(Roles = "Client")]
    public class BeneficiaryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
