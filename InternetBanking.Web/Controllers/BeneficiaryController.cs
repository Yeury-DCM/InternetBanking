 
﻿using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.Services;
using InternetBanking.Core.Application.ViewModels.BeneficiaryVMS;

﻿using InternetBanking.Core.Application.Enums;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Web.Controllers
{
 
    [Route("api/[controller]")]
    [ApiController]

    
    [Authorize(Roles = "Client")]

    public class BeneficiaryController : Controller
    {
        private readonly BeneficiaryService _beneficiaryService;

        public BeneficiaryController(BeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;
        }

        // GET: api/Beneficiary
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var beneficiaries = await _beneficiaryService.GetAllViewModel();
                return Ok(beneficiaries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Beneficiary/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var beneficiary = await _beneficiaryService.GetById(id);
                if (beneficiary == null) return NotFound("Beneficiary not found.");

                return Ok(beneficiary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Beneficiary
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] SaveBeneficiaryViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            try
            {
                await _beneficiaryService.Add(vm);
                return Ok("Beneficiary added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Beneficiary/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SaveBeneficiaryViewModel vm)
        {
            if (!ModelState.IsValid || vm.Id != id)
                return BadRequest("Invalid data or ID mismatch.");

            try
            {
                await _beneficiaryService.Update(vm);
                return Ok("Beneficiary updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Beneficiary/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _beneficiaryService.Delete(id);
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
