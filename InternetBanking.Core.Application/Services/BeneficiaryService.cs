using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.BeneficiaryVMS;
using InternetBanking.Core.Domain.Entities;

public class BeneficiaryService
{
    private readonly IBeneficiaryRepository _beneficiaryRepository;
    private readonly IAccountService _accountService;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, IAccountService accountService, IProductRepository productRepository, IMapper mapper)
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

        if (product.User == null || !product.User.Status)
        {
            throw new Exception("Usuario relacionado con el número de cuenta está inactivo.");
        }

        var beneficiary = _mapper.Map<Beneficiary>(vm);
        beneficiary.ProductID = product.Id;

        await _beneficiaryRepository.AddAsync(beneficiary);
    }

    public async Task<List<BeneficiaryViewModel>> GetAllBeneficiaries(int userId)
    {
        var beneficiaries = await _beneficiaryRepository.GetAllWithIncludesAsync(new List<string> { "Product", "Product.User" });
        var userBeneficiaries = beneficiaries.Where(b => b.UserID == userId).ToList();

        return _mapper.Map<List<BeneficiaryViewModel>>(userBeneficiaries);
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
}
