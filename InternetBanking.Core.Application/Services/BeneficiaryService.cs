using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class BeneficiaryService
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, UserManager<IdentityUser> userManager)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _userManager = userManager;
        }

        public async Task Add(SaveBeneficiaryViewModel vm)
        {
            Beneficiary beneficiary = new()
            {
                Id = vm.Id, // In-Built Copilot help - in case of needed removal
                UserID = vm.UserId,
                ProductID = vm.ProductId
            };

            await _beneficiaryRepository.AddAsync(beneficiary);
        }

        public async Task Update(SaveBeneficiaryViewModel vm)
        {
            var beneficiary = await _beneficiaryRepository.GetByIdAsync(vm.Id);

            if (beneficiary == null)
            {
                throw new Exception("Beneficiary not found.");
            }

            beneficiary.UserID = vm.UserId;
            beneficiary.ProductID = vm.ProductId;

            await _beneficiaryRepository.UpdateAsync(beneficiary, beneficiary.Id);
        }

        public async Task<SaveBeneficiaryViewModel> GetById(int id)
        {
            var beneficiary = await _beneficiaryRepository.GetByIdAsync(id);
            if (beneficiary == null) return null;

            var user = await _userManager.FindByIdAsync(beneficiary.UserID.ToString());
            if (user == null) return null;

            return new SaveBeneficiaryViewModel
            {
                Id = beneficiary.Id,
                UserId = beneficiary.UserID,
                ProductId = beneficiary.ProductID,
                BeneficiaryFirstName = user.UserName 
            };
        }

        public async Task<List<BeneficiaryViewModel>> GetAllViewModel()
        {
            var beneficiaries = await _beneficiaryRepository.GetAllAsync();

            return beneficiaries.Select(async b =>
            {
                var user = await _userManager.FindByIdAsync(b.UserID.ToString());
                return new BeneficiaryViewModel
                {
                    Id = b.Id,
                    UserId = b.UserID,
                    ProductId = b.ProductID,
                    BeneficiaryFullName = user?.UserName 
                };
            })
            .Select(t => t.Result)
            .ToList();
        }

        public async Task Delete(int id)
        {
            var beneficiary = await _beneficiaryRepository.GetByIdAsync(id);

            if (beneficiary == null)
            {
                throw new KeyNotFoundException($"Beneficiary with ID {id} not found.");
            }

            await _beneficiaryRepository.DeleteAsync(beneficiary);
        }

    }
}
