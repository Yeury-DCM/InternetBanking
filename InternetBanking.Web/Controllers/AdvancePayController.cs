//using AutoMapper;
//using InternetBanking.Core.Application.Services;
//using InternetBanking.Core.Application.ViewModels.AdvanceCashVMS;
//using Microsoft.AspNetCore.Mvc;

//namespace InternetBanking.Web.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AdvanceCashController : ControllerBase
//    {
//        private readonly AdvanceCashService _advanceCashService;
//        private readonly IMapper _mapper;

//        public AdvanceCashController(AdvanceCashService advanceCashService, IMapper mapper)
//        {
//            _advanceCashService = advanceCashService;
//            _mapper = mapper;
//        }

//        [HttpPost("AdvanceCash")]
//        public async Task<IActionResult> AdvanceCash([FromBody] SaveAdvanceCashViewModel vm)
//        {
//            if (vm == null)
//                return BadRequest("Request data is missing.");

//            if (!ModelState.IsValid)
//                return BadRequest("Los datos ingresados no son válidos.");

//            try
//            {
//                await _advanceCashService.AdvanceCash(vm);
//                return Ok("El avance de efectivo se ha realizado con éxito.");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpGet("CreditCards/{userId}")]
//        public async Task<IActionResult> GetCreditCards(string userId)
//        {
//            try
//            {
//                var creditCards = await _advanceCashService.GetUserCreditCards(userId);
//                var creditCardVMs = _mapper.Map<List<AdvanceCashViewModel>>(creditCards);

//                if (creditCardVMs.Count == 0)
//                    return NotFound("No se encontraron tarjetas de crédito para este usuario.");

//                return Ok(creditCardVMs);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error del servidor: {ex.Message}");
//            }
//        }

//        [HttpGet("SavingsAccounts/{userId}")]
//        public async Task<IActionResult> GetSavingsAccounts(string userId)
//        {
//            try
//            {
//                var savingsAccounts = await _advanceCashService.GetUserSavingsAccounts(userId);
//                var savingsAccountVMs = _mapper.Map<List<AdvanceCashViewModel>>(savingsAccounts);

//                if (savingsAccountVMs.Count == 0)
//                    return NotFound("No se encontraron cuentas de ahorro para este usuario.");

//                return Ok(savingsAccountVMs);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error del servidor: {ex.Message}");
//            }
//        }
//    }