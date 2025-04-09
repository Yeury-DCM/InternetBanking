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

        public override async Task<SaveProductViewModel> Add(SaveProductViewModel viewModel)
        {
            if(viewModel.ProductTypeID ==3)
            {
                var products = await _productRepository.GetAllWithIncludesAsync(new List<string> { "productType", "transactions" });
                var principalAccount = products.Where(p => p.UserID == viewModel.UserID).FirstOrDefault(P => P.IsPrincipal);



                principalAccount.Balance += viewModel.Balance;
                SaveProductViewModel saveProductViewModel = _mapper.Map<SaveProductViewModel>(principalAccount);
                await base.Update(saveProductViewModel);
            }

            return await base.Add(viewModel);
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

        public async Task<DeleteProductResponse> DeleteProductAsync(int productId)
        {

            DeleteProductResponse deleteProductResponse = new DeleteProductResponse() { IsSucess = true };
            Product product = await _productRepository.GetByIdAsync(productId);
            deleteProductResponse.UserId = product.UserID;

            try
            {
                //Saving Accounts
                if (product.ProductTypeID == 1)
                {

                    if(product.IsPrincipal)
                    {
                        deleteProductResponse.IsSucess = false;
                        deleteProductResponse.ErrorMessage = "No se puede borrar la cuenta principal.";
                        return deleteProductResponse;
                    }


                    Product principalAccount = (await _productRepository.GetAllAsync()).Where(p => p.UserID == product.UserID).FirstOrDefault(p => p.IsPrincipal)!;
                    if(principalAccount != null)
                    {
                        await AddAmount(principalAccount.Id, product.Balance);
                    }

                }

                //Credit Cards 
                if (product.ProductTypeID == 2)
                {
                    if (product.Balance > 0)
                    {
                        deleteProductResponse.ErrorMessage = "Esta tarjeta tiene balance pendiente.";
                        deleteProductResponse.IsSucess = false;
                        return deleteProductResponse;
                    }
                }

                //Loan
                if (product.ProductTypeID == 3)
                {
                    if (product.Balance > 0)
                    {
                        deleteProductResponse.ErrorMessage = "Esta prestamo tiene balance pendiente.";
                        deleteProductResponse.IsSucess = false;
                        return deleteProductResponse;
                    }
                }

                await base.DeleteById(productId);

            }
            catch(Exception ex)
            {

            }
           

            return deleteProductResponse;
        }

    }
}
