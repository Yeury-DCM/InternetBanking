
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.Services;
using InternetBanking.Core.Application.ViewModels.BeneficiaryVMS;

using InternetBanking.Core.Application.Enums;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Client")]

    public class BeneficiaryController : ControllerBase
    {
        private readonly BeneficiaryService _beneficiaryService;

        public BeneficiaryController(BeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;
        }

        [HttpGet("GetAll/{userId}")]
        public async Task<IActionResult> GetAll(string userId)
        {
            try
            {
                var beneficiaries = await _beneficiaryService.GetAllBeneficiaries(userId);
                if (beneficiaries == null || beneficiaries.Count == 0)
                    return NotFound("No beneficiaries found for the given user.");

                return Ok(beneficiaries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] SaveBeneficiaryViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            try
            {
                await _beneficiaryService.AddBeneficiary(vm);
                return Ok("Beneficiary added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _beneficiaryService.DeleteBeneficiary(id);
                return Ok("Beneficiary deleted successfully.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Beneficiary with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
