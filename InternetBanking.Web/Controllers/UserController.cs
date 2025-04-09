using AutoMapper;
using InternetBanking.Core.Application.Dtos;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.ProductVMS;
using InternetBanking.Core.Application.ViewModels.UserVMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternetBanking.Web.Controllers
{
    [Authorize]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        IAccountService _accountService;
        IMapper _mapper;
        IProductService _productService;
        public UserController(IAccountService accountService, IMapper mapper, IProductService productService)
        {
            _accountService = accountService;
            _mapper = mapper;
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {

            return View(await _accountService.GetAllUserViewModelsAsync());
        }


        public IActionResult Add()
        {
            ViewBag.UserTypes = Enum.GetValues<Roles>();

            return View("SaveUser", new SaveUserViewModel());
        }



        [HttpPost]
        public async Task<IActionResult> Add(SaveUserViewModel saveUserViewModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                ViewBag.UserTypes = Enum.GetValues<Roles>();

                return View("SaveUser", saveUserViewModel);
            }

            SaveUserRequest request = _mapper.Map<SaveUserRequest>(saveUserViewModel);
            SaveUserResponse response = await _accountService.CreateUser(request);

            if (!response.IsSucess)
            {
                saveUserViewModel.ErrorMessage = response.ErrorMessage;
                saveUserViewModel.IsSucess = false;
                ViewBag.UserTypes = Enum.GetValues<Roles>();

                return View("SaveUser", saveUserViewModel);
            }

            if (saveUserViewModel.UserType == Roles.Client)
            {
                SaveProductViewModel saveProductViewModel = new()
                {
                    Balance = saveUserViewModel.Amount,
                    UserID = response.UserId,
                    IsPrincipal = true,
                    ProductTypeID = 1,
                    ProductNumber = AccountNumberGenerator.Generate()
                };

                await _productService.Add(saveProductViewModel);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Activate(string userId)
        {
            await _accountService.ActivateUser(userId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate(string userId)
        {
            await _accountService.DeactivateUser(userId);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string userId)
        {
            ViewBag.UserTypes = Enum.GetValues<Roles>();

            UserViewModel user = await _accountService.GetUserViewModelByIdAsync(userId);

            SaveUserViewModel saveUserViewModel = _mapper.Map<SaveUserViewModel>(user);

            return View("SaveUser", saveUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUserViewModel saveUserViewModel)
        {
            if (string.IsNullOrEmpty(saveUserViewModel.Password) && string.IsNullOrEmpty(saveUserViewModel.ConfirmPassword))
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }

        
            if (!ModelState.IsValid)
            {
                ViewBag.UserTypes = Enum.GetValues<Roles>();
                return View("SaveUser", saveUserViewModel);
            }

            await _accountService.UpdateUserAsync(saveUserViewModel);

            if (saveUserViewModel.Amount > 0)
            {
                var principalAccount = await _productService.GetPrincipalAccountByUserId(saveUserViewModel.Id);
                await _productService.AddAmount(principalAccount.Id, saveUserViewModel.Amount);
            }

            return RedirectToAction("Index");
        }


    }
}
