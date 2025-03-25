using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Web.Controllers
{
    public class BeneficiaryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
