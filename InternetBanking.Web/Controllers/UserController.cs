using InternetBanking.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Web.Controllers
{
    public class UserController : Controller
    {
        IAccountService _accountService;
        public UserController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _accountService.GetAllUserViewModelsAsync());
        }
    }
}
