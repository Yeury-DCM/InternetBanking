using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Services;
using InternetBanking.Core.Application.ViewModels.AdvanceCashVMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Web.Controllers
{
    [Authorize]
    public class AdvancePayController : Controller
    {
        private readonly AdvanceCashService _advanceCashService;
        private readonly IProductRepository _productRepository;

        public AdvancePayController(AdvanceCashService advanceCashService, IProductRepository productRepository)
        {
            _advanceCashService = advanceCashService;
            _productRepository = productRepository;
        }

        // GET: /AdvanceCash
        public async Task<IActionResult> Index()
        {
            var userId = "77559e0f-1a4b-4d08-9393-02b25bc13527"; // Hardcode para testing

            // Obtener tarjetas de crédito del usuario
            var creditCards = await _productRepository.GetByUserIdAndTypeAsync(userId, "Tarjeta de Credito");
            ViewBag.CreditCards = new SelectList(creditCards, "Id", "Name");
            ViewBag.CreditCardsList = creditCards.ToList(); // Agregar la lista completa

            // Obtener cuentas de ahorro del usuario
            var savingsAccounts = await _productRepository.GetByUserIdAndTypeAsync(userId, "Cuenta de ahorro");
            ViewBag.SavingsAccounts = new SelectList(savingsAccounts, "Id", "Name");
            ViewBag.SavingsAccountsList = savingsAccounts.ToList(); // Agregar la lista completa

            var model = new SaveAdvanceCashViewModel();
            return View("AdvancePay", model);
        }


        // POST: /AdvanceCash
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SaveAdvanceCashViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var userId = "77559e0f-1a4b-4d08-9393-02b25bc13527"; // Hardcode para testing

                // Recargar listas desplegables en caso de error
                var creditCards = await _productRepository.GetByUserIdAndTypeAsync(userId, "Tarjeta de Credito");
                ViewBag.CreditCards = new SelectList(creditCards, "Id", "Name");

                var savingsAccounts = await _productRepository.GetByUserIdAndTypeAsync(userId, "Cuenta de ahorro");
                ViewBag.SavingsAccounts = new SelectList(savingsAccounts, "Id", "Name");

                return RedirectToAction("Index", "Home");

            }

            try
            {
                // Convertir el modelo de la vista al modelo de servicio
                var serviceModel = new AdvanceCashViewModel
                {
                    CreditCardId = model.CreditCardId,
                    SavingsAccountId = model.SavingsAccountId,
                    Amount = model.Amount,
                    InterestRate = model.InterestRate
                };

                // Realizar el avance de efectivo
                await _advanceCashService.AdvanceCash(serviceModel);

                TempData["SuccessMessage"] = "Avance de efectivo realizado con éxito.";
                return RedirectToAction("Details", "Account");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var userId = "77559e0f-1a4b-4d08-9393-02b25bc13527"; // Hardcode para testing

                // Recargar listas desplegables en caso de error
                var creditCards = await _productRepository.GetByUserIdAndTypeAsync(userId, "Tarjeta de Credito");
                ViewBag.CreditCards = new SelectList(creditCards, "Id", "Name");

                var savingsAccounts = await _productRepository.GetByUserIdAndTypeAsync(userId, "Cuenta de ahorro");
                ViewBag.SavingsAccounts = new SelectList(savingsAccounts, "Id", "Name");

                return View(model);
            }
        }

        // GET: /AdvanceCash/Confirm
        public async Task<IActionResult> Confirm(SaveAdvanceCashViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            try
            {
                // Obtener información de productos para mostrar en confirmación
                var creditCard = await _productRepository.GetByIdAsync(model.CreditCardId);
                var savingsAccount = await _productRepository.GetByIdAsync(model.SavingsAccountId);

                if (creditCard == null || savingsAccount == null)
                {
                    throw new Exception("Tarjeta de crédito o cuenta de ahorro no encontrada.");
                }

                ViewBag.CreditCardName = creditCard.ProductNumber;
                ViewBag.SavingsAccountName = savingsAccount.ProductNumber;

                // Calcular interés y monto total
                decimal interest = model.Amount * (model.InterestRate / 100);
                decimal totalDebt = model.Amount + interest;

                ViewBag.Interest = interest;
                ViewBag.TotalDebt = totalDebt;

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index");
            }
        }

        // POST: /AdvanceCash/Confirm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPost(SaveAdvanceCashViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Product");
            }

            try
            {
                // Convertir el modelo de la vista al modelo de servicio
                var serviceModel = new AdvanceCashViewModel
                {
                    CreditCardId = model.CreditCardId,
                    SavingsAccountId = model.SavingsAccountId,
                    Amount = model.Amount,
                    InterestRate = model.InterestRate
                };

                // Realizar el avance de efectivo
                await _advanceCashService.AdvanceCash(serviceModel);

                TempData["SuccessMessage"] = "Avance de efectivo realizado con éxito.";
                return RedirectToAction("Index", "Product");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}