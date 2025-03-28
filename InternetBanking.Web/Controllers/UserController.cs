using AutoMapper;
using InternetBanking.Core.Application.Dtos;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Services;
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
        public UserController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
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

            return RedirectToAction("Index");
        }

    }
}
