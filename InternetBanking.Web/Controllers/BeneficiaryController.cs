
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.Services;
using InternetBanking.Core.Application.ViewModels.BeneficiaryVMS;

using InternetBanking.Core.Application.Enums;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BeneficiaryController : Controller
    {
        private readonly IBeneficiaryService _beneficiaryService;

        public BeneficiaryController(IBeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirst("Id")?.Value;
            var beneficiaries = await _beneficiaryService.GetAllBeneficiaries(userId);
            return View(beneficiaries);
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            return View(new SaveBeneficiaryViewModel());
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(SaveBeneficiaryViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                vm.UserId = User.FindFirst("Id")?.Value;
                await _beneficiaryService.AddBeneficiary(vm);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("AccountNumber", ex.Message);
                return View(vm);
            }
        }

        [HttpPost]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _beneficiaryService.DeleteBeneficiary(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
    }
}