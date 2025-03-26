using InternetBanking.Core.Application.ViewModels.TransferVMS;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Web.Controllers
{
    public class TransferController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.UserAccounts = new List<string>() { "2930193913 - Cuenta 1", "13161264246 - Cuenta 2", "234251235 - Cuenta 3" };
            return View(new MakeTransferViewModel());
        }

        [HttpPost]
        public IActionResult MakeTransfer(MakeTransferViewModel makeTransferViewModel)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.UserAccounts = new List<string>() { "2930193913 - Cuenta 1", "13161264246 - Cuenta 2", "234251235 - Cuenta 3" };
                return View("Index", makeTransferViewModel);
            }


            return RedirectToAction("Index", new MakeTransferViewModel());
        }
    }
}
