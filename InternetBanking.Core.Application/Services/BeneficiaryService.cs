using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Helpers;

namespace InternetBanking.Core.Application.Services
{
    public class BeneficiaryService : GenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>, IBeneficiaryService
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userViewModel;
        private readonly IMapper _mapper;

        public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(beneficiaryRepository, mapper)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public override async Task<SaveBeneficiaryViewModel> Add(SaveBeneficiaryViewModel vm)
        {
            vm.UserId = userViewModel != null ? userViewModel.Id : vm.UserId;
            return await base.Add(vm);
        }

        public override async Task Update(SaveBeneficiaryViewModel vm, int id)
        {
            vm.UserId = userViewModel != null ? userViewModel.Id : vm.UserId;
            await base.Update(vm, id);
        }

        public async Task<List<BeneficiaryViewModel>> GetAllViewModelWithInclude()
        {
            var beneficiaryList = _beneficiaryRepository.GetAllQueryWithInclude(new List<string> { "user", "account" });

            if (userViewModel != null)
            {
                beneficiaryList = beneficiaryList.Where(beneficiary => beneficiary.UserID == userViewModel.Id);
            }

            return await beneficiaryList.Select(beneficiary => new BeneficiaryViewModel
            {
                Id = beneficiary.Id,
                UserId = beneficiary.UserID,
                ProductId = beneficiary.ProductID,
                BeneficiaryFullName = $"{beneficiary.user.Name} {beneficiary.user.LastName}",
                ProductType = beneficiary.account.ProductType,
                AccountNumber = beneficiary.account.ProductNumber
            }).ToListAsync();
        }

        public async Task<List<BeneficiaryViewModel>> GetAllBeneficiaries(
        string beneficiaryFirstName = null,
        string beneficiaryLastName = null,
        string accountNumber = null,
        string productType = null,
        int? userId = null,
        int? productId = null,
        DateTime? addedDateFrom = null,
        DateTime? addedDateTo = null)
        {
            var beneficiaryList = _beneficiaryRepository.GetAllQueryWithInclude(new List<string> { "user", "account" });

            if (userId != null)
            {
                beneficiaryList = beneficiaryList.Where(beneficiary => beneficiary.UserID == userId.Value);
            }
            if (!string.IsNullOrEmpty(beneficiaryFirstName))
            {
                beneficiaryList = beneficiaryList.Where(beneficiary => beneficiary.user.Name.Contains(beneficiaryFirstName));
            }
            if (!string.IsNullOrEmpty(beneficiaryLastName))
            {
                beneficiaryList = beneficiaryList.Where(beneficiary => beneficiary.user.LastName.Contains(beneficiaryLastName));
            }
            if (!string.IsNullOrEmpty(accountNumber))
            {
                beneficiaryList = beneficiaryList.Where(beneficiary => beneficiary.account.ProductNumber.Contains(accountNumber));
            }
            if (!string.IsNullOrEmpty(productType))
            {
                beneficiaryList = beneficiaryList.Where(beneficiary => beneficiary.account.ProductType.Contains(productType));
            }
            

            return await beneficiaryList.Select(beneficiary => new BeneficiaryViewModel
            {
                Id = beneficiary.Id,
                UserId = beneficiary.UserID,
                ProductId = beneficiary.ProductID,
                BeneficiaryFullName = $"{beneficiary.account.User.Name}",
                ProductType = beneficiary.account.ProductType,
                AccountNumber = beneficiary.account.ProductNumber
            }).ToListAsync();
        }

    }

}
