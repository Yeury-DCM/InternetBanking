using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class BeneficiaryService
{
    private readonly IBeneficiaryRepository _beneficiaryRepository;
    private readonly IAccountService _accountService;

    public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, IAccountService accountService)
    {
        _beneficiaryRepository = beneficiaryRepository;
        _accountService = accountService;
    }

    public async Task Add(SaveBeneficiaryViewModel vm)
    {
        var beneficiary = new Beneficiary
        {
            Id = vm.Id,
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

        var user = await _accountService.GetUserByIdAsync(beneficiary.UserID);
        if (user == null) return null;

        return new SaveBeneficiaryViewModel
        {
            Id = beneficiary.Id,
            UserId = beneficiary.UserID,
            ProductId = beneficiary.ProductID,
            BeneficiaryFirstName = user.FirstName,
            BeneficiaryLastName = user.LastName
        };
    }

    public async Task<List<BeneficiaryViewModel>> GetAllViewModel()
    {
        var beneficiaries = await _beneficiaryRepository.GetAllAsync();

        var beneficiaryViewModels = new List<BeneficiaryViewModel>();
        foreach (var beneficiary in beneficiaries)
        {
            var user = await _accountService.GetUserByIdAsync(beneficiary.UserID);
            if (user == null) continue;

            beneficiaryViewModels.Add(new BeneficiaryViewModel
            {
                Id = beneficiary.Id,
                UserId = beneficiary.UserID,
                ProductId = beneficiary.ProductID,
                BeneficiaryFullName = $"{user.FirstName} {user.LastName}",
                ProductType = beneficiary.Account?.Type ?? "Unknown",
                AccountNumber = beneficiary.Account?.Number ?? "Unknown"
            });
        }

        return beneficiaryViewModels;
    }
}
