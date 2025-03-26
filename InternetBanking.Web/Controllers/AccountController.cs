using InternetBanking.Core.Application.Dtos;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.AccountVMS;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(IAccountService accountService, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {

            if(!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            AuthenticationRequest request = new() { UserName = loginViewModel.UserName, Password = loginViewModel.Password };

            AuthenticationResponse response = await _accountService.AuthenticateAsync(request);

            if(!response.IsSucess)
            {
                loginViewModel.IsSucess = false;
                loginViewModel.ErrorMessage = response.ErrorMessage;
                return View(loginViewModel);
            }

            _httpContextAccessor.HttpContext!.Session.Set<AuthenticationResponse>("user", response);

            if(response.Roles.Any(r => r == Roles.Client.ToString()))
            {
                return RedirectToRoute(new { action = "Index", controller = "Product" });

            }else if(response.Roles.Any(r => r == Roles.Admin.ToString()))
            {
                return RedirectToRoute(new { action = "Index", controller = "Dashboard" });

            }

            return View(loginViewModel);


        }

        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
