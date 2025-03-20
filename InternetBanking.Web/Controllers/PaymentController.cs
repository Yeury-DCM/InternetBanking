using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Web.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
