using Microsoft.AspNetCore.Mvc;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.BeneficiaryVMS;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace InternetBanking.Web.Controllers
{
    public class BeneficiaryController : Controller
    {
        private readonly IBeneficiaryService _beneficiaryService;

        public BeneficiaryController(IBeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;
        }

        public async Task<IActionResult> Index()
        {
            string userId = "77559e0f-1a4b-4d08-9393-02b25bc13527";
            var beneficiaries = await _beneficiaryService.GetAllBeneficiaries(userId);
            return View(beneficiaries);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("SaveBeneficiary", new SaveBeneficiaryViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveBeneficiaryViewModel vm)
        {
            Console.WriteLine("Create action hit with data: " + vm.BeneficiaryFirstName); //debug purposes

            if (!ModelState.IsValid)
            {
                return View("SaveBeneficiary", vm);
            }

            try
            {
                //vm.UserId = User.FindFirst("Id")?.Value;
                vm.UserId = "77559e0f-1a4b-4d08-9393-02b25bc13527";
                await _beneficiaryService.AddBeneficiary(vm);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("AccountNumber", ex.Message);
                return View("SaveBeneficiary", vm);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _beneficiaryService.DeleteBeneficiary(id);
            }
            catch (Exception)
            {
                // Log error or handle it appropriately
            }
            return RedirectToAction("Index");
        }
    }
}