using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.ProductVMS;
using InternetBanking.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using InternetBanking.Core.Application.Dtos;
using InternetBanking.Core.Application.Helpers;


namespace InternetBanking.Core.Application.Services
{
    public class ProductService : GenericService<SaveProductViewModel, ProductViewModel, Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _authenticationResponse;
         

        public ProductService(IProductRepository productRepository, IMapper mapper, IGenericRepository<Product> genericRepository, IHttpContextAccessor httpContextAccessor) : base(genericRepository, mapper)
        {

            _productRepository = productRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _authenticationResponse = httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user")!;
        }


        public async Task<List<ProductViewModel>> GetAll()
        {
            
            var products = await _productRepository.GetAllWithIncludesAsync(new List<string> { "productType", "transactions" });
            var productsByUserLoggedIn = products.Where(p => p.UserID == _authenticationResponse.Id);

            return _mapper.Map<List<ProductViewModel>>(productsByUserLoggedIn);
        }

        public async Task AddAmount(int productId, decimal amount)
        {
            Product product = await _productRepository.GetByIdAsync(productId);
            product.Balance += amount;

            await _productRepository.UpdateAsync(product, productId);
        }

        public async Task<ProductViewModel> GetPrincipalAccountByUserId(string userId)
        {
            var products = await _productRepository.GetAllAsync();
            Product? principalAccount = products.Where(p => p.UserID == userId)
                                       .FirstOrDefault(p => p.IsPrincipal);

            ProductViewModel principalAccoutViewModel = _mapper.Map<ProductViewModel>(principalAccount);

            return principalAccoutViewModel;




        }

    }
}
