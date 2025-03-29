using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.BeneficiaryVMS;
using InternetBanking.Core.Application.ViewModels.UserVMS;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Services
{
    public class BeneficiaryService : GenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>, IBeneficiaryService
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IAccountService _accountService;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, IAccountService accountService,
            IProductRepository productRepository, IMapper mapper) : base(beneficiaryRepository, mapper)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _accountService = accountService;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task AddBeneficiary(SaveBeneficiaryViewModel vm)
        {
            var products = await _productRepository.GetAllWithIncludesAsync(new List<string> { "ProductType", "User" });
            var product = products.FirstOrDefault(p => p.ProductNumber == vm.AccountNumber);

            if (product == null)
            {
                throw new Exception("El número de cuenta proporcionado no existe.");
            }

            UserViewModel user = await _accountService.GetUserViewModelByIdAsync(product.UserID);

            if (user == null || user.IsActive)
            {
                throw new Exception("Usuario relacionado con el número de cuenta está inactivo.");
            }

            var beneficiary = _mapper.Map<Beneficiary>(vm);
            beneficiary.ProductID = product.Id;
            await _beneficiaryRepository.AddAsync(beneficiary);
        }

        public async Task DeleteBeneficiary(int beneficiaryId)
        {
            var beneficiary = await _beneficiaryRepository.GetByIdAsync(beneficiaryId);
            if (beneficiary == null)
            {
                throw new Exception("Beneficiario no encontrado.");
            }
            await _beneficiaryRepository.DeleteAsync(beneficiary);
        }

        public async Task<List<BeneficiaryViewModel>> GetAllBeneficiaries(string userId)
        {
            var beneficiaries = await _beneficiaryRepository.GetAllWithIncludesAsync(new List<string> { "account" });
            var userBeneficiaries = beneficiaries.Where(b => b.UserID == userId).ToList();
            return _mapper.Map<List<BeneficiaryViewModel>>(userBeneficiaries);
        }

        // IGenericService implementation methods
        public override async Task<SaveBeneficiaryViewModel> Add(SaveBeneficiaryViewModel viewModel)
        {
            try
            {
                await AddBeneficiary(viewModel);
                return viewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public new async Task<List<BeneficiaryViewModel>> GetAll()
        {
            var entities = await _beneficiaryRepository.GetAllAsync();
            return _mapper.Map<List<BeneficiaryViewModel>>(entities);
        }

        public new async Task<BeneficiaryViewModel> GetById(int id)
        {
            var entity = await _beneficiaryRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new Exception($"Beneficiario con ID {id} no encontrado.");
            }
            return _mapper.Map<BeneficiaryViewModel>(entity);
        }

        public new async Task Update(SaveBeneficiaryViewModel viewModel)
        {
            var entity = await _beneficiaryRepository.GetByIdAsync(viewModel.Id);
            if (entity == null)
            {
                throw new Exception($"Beneficiario con ID {viewModel.Id} no encontrado.");
            }

            entity = _mapper.Map(viewModel, entity);
            await _beneficiaryRepository.UpdateAsync(entity, viewModel.Id);
        }

        public new async Task DeleteById(int id)
        {
            await DeleteBeneficiary(id);
        }
    }
}