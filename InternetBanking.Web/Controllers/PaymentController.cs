using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public PaymentController(PaymentServiceFactory paymentServiceFactory, IProductService productService, IMapper mapper)
        {
            _paymentServiceFactory = paymentServiceFactory;
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<IActionResult> ProcessPayment(SavePaymentViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View($"{vm.PaymentType}", vm);
                }

                // Obtener el servicio de pago correcto
                var paymentService = _paymentServiceFactory.GetService(vm.PaymentType);
                var confirmPayment = await paymentService.ProcessPaymentData(vm);

                return View("Confirmation", confirmPayment);
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToRoute(new { controller = "Product", action = "Index" });
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al procesar el pago. Inténtalo de nuevo.";
                return RedirectToRoute(new { controller = "Product", action = "Index" });
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
               
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToRoute(new { controller = "Product", action = "Index" });
            }
            catch (Exception)
            {
              
                TempData["ErrorMessage"] = "Ocurrió un error al procesar el pago. Inténtalo de nuevo.";
                return RedirectToRoute(new { controller = "Product", action = "Index" });
            }
        }
        public async Task<IActionResult> Express()
        {
            SavePaymentViewModel vm = new SavePaymentViewModel();
            vm.products = await _productService.GetAll();
            return View(vm);
        }
        public async Task<IActionResult> Transfer()
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
