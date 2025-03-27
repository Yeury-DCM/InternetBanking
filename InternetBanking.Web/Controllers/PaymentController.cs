using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.Services.Factory;
using InternetBanking.Core.Application.ViewModels.PayementVMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Web.Controllers
{
    
    [Authorize(Roles = "Client")]

    public class PaymentController : Controller
    {
        private readonly PaymentServiceFactory _paymentServiceFactory;
        private readonly IProductService _productService;

        public PaymentController(PaymentServiceFactory paymentServiceFactory, IProductService productService)
        {
            _paymentServiceFactory = paymentServiceFactory;
            _productService = productService;
        }

        public async Task<IActionResult> ProcessPayment(SavePaymentViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToRoute(new { controller = "Payment", action = "Express" });
                }

                // Obtener el servicio de pago correcto
                var paymentService = _paymentServiceFactory.GetService(vm.PaymentType);
                var confirmPayment = await paymentService.ProcessPaymentData(vm);

                return View("Confirmation", confirmPayment);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToRoute(new { controller = "Payment", action = "Express" });
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al procesar el pago. Inténtalo de nuevo.");
                return RedirectToRoute(new { controller = "Payment", action = "Express" });
            }
        }

        public async Task<IActionResult> ConfirmPayment(PaymentViewModel vm)
        {
            try
            {
                var paymentService = _paymentServiceFactory.GetService(vm.PaymentType);
                await paymentService.ConfirmPayment(vm);
                return RedirectToRoute(new { controller = "Product", action = "Index" });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToRoute(new { controller = "Payment", action = "Express" });
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al procesar el pago. Inténtalo de nuevo.");
                return RedirectToRoute(new { controller = "Payment", action = "Express" });
            }
        }
        public async Task<IActionResult> Express()
        {
            SavePaymentViewModel vm = new SavePaymentViewModel();
            vm.products = await _productService.GetAll();
            return View(vm);
        }

        public async Task<IActionResult> CreditCard()
        {
            SavePaymentViewModel vm = new SavePaymentViewModel();
            vm.products = await _productService.GetAll();
            return View(vm);
        }

        public async Task<IActionResult> Loan()
        {
            SavePaymentViewModel vm = new SavePaymentViewModel();
            vm.products = await _productService.GetAll();
            return View(vm);
        }

        public async Task<IActionResult> Beneficiary()
        {
            SavePaymentViewModel vm = new SavePaymentViewModel();
            vm.products = await _productService.GetAll();
            return View(vm);
        }

    }

}
