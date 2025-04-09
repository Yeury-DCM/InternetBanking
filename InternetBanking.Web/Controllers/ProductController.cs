using Azure;
using InternetBanking.Core.Application.Dtos;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.ProductVMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Web.Controllers
{
    
    public class ProductController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IProductService _productService;
        private readonly IAccountService _accountService;
        public ProductController(IDashboardService dashboardService, IProductService productService, IAccountService accountService)
        {
            _dashboardService = dashboardService;
            _productService = productService;
            _accountService = accountService;
        }


        // GET: ProductController
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Index()
        {
            var vm = await _dashboardService.GetUserProductsInfo();
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminView(string userId, string? errorMessage = null)
        {
            var vm = await _dashboardService.GetUserProductsInfo(userId);
            var user = await _accountService.GetUserViewModelByIdAsync(userId);
            if(errorMessage!= null)
            {
                ViewBag.ErrorMessage = errorMessage;
            }
            ViewBag.UserId = userId;  
            ViewBag.FullName = $"{user.FirstName} {user.LastName}";
            return View("Index", vm);
        }
       
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public ActionResult Create(int productType, string userId)
        {
            return View("SaveProduct", new SaveProductViewModel() { ProductTypeID = productType, UserID = userId });
        }

        // POST: ProductController/Create
        [HttpPost]
        public async Task<ActionResult> Create(SaveProductViewModel saveProductViewModel)
        {
            try
            {
                saveProductViewModel.ProductNumber = AccountNumberGenerator.Generate();
                await _productService.Add(saveProductViewModel);
                return RedirectToAction("AdminView", new { userId = saveProductViewModel.UserID });
            }
            catch
            {
                return View();
            }
        }

           
       

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            DeleteProductResponse response = new();
            try
            {
                 response = await _productService.DeleteProductAsync(id);

                if(!response.IsSucess)
                {
                   return RedirectToAction("AdminView", new { userId = response.UserId, errorMessage = response.ErrorMessage});

                }
            }
            catch
            {
            }

            return RedirectToAction("AdminView", new { userId = response.UserId });
        }
    }
}
