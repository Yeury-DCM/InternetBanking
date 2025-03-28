using InternetBanking.Core.Application.Dtos;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.AccountVMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace InternetBanking.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _authenticationResponse;

        public AccountController(IAccountService accountService, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
            _authenticationResponse = httpContextAccessor.HttpContext!.Session.Get<AuthenticationResponse>("user")!;

        }

        [AllowAnonymous]
        public async Task<IActionResult> LogIn()
        {
            
            bool isAuthenticated = User.Identity!.IsAuthenticated;
            bool isAdmin = User.IsInRole("Admin");
            var userSession = _httpContextAccessor.HttpContext!.Session.Get<AuthenticationResponse>("user");

            if (isAuthenticated && userSession == null)
            {
                return await LogOut();
               
            }

            if (!isAuthenticated)
            {
                return View(new LoginViewModel());
            }

            if (isAdmin)
            {
                return RedirectToRoute(new { action = "Index", controller = "Dashboard" });
            }
            else
            {
                return RedirectToRoute(new { action = "Index", controller = "Product" });
            }

        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            AuthenticationRequest request = new() { UserName = loginViewModel.UserName, Password = loginViewModel.Password };

            AuthenticationResponse response = await _accountService.AuthenticateAsync(request);

            if (!response.IsSucess)
            {
                loginViewModel.IsSucess = false;
                loginViewModel.ErrorMessage = response.ErrorMessage;
                return View(loginViewModel);
            }

            _httpContextAccessor.HttpContext!.Session.Set<AuthenticationResponse>("user", response);

            if (response.Roles.Any(r => r == Roles.Client.ToString()))
            {
                return RedirectToRoute(new { action = "Index", controller = "Product" });

            }
            else if (response.Roles.Any(r => r == Roles.Admin.ToString()))
            {
                return RedirectToRoute(new { action = "Index", controller = "Dashboard" });

            }

            return View(loginViewModel);


        }

        public async Task<IActionResult> LogOut()
        {
            await _accountService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToAction("LogIn");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }




    }
}
