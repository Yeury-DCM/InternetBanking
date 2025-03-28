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
            ModelState.Remove("UserId");
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
                return View(saveUserViewModel);
            }

            if(saveUserViewModel.UserType == Roles.Client)
            {
                SaveProductViewModel saveProductViewModel = new()
                {
                    Balance = saveUserViewModel.InitialAmount,
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
        public async Task <IActionResult> Activate (string userId)
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

    }
}
