using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.ProductVMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {

            _productRepository = productRepository;
            _mapper = mapper;
        }

        public Task<SaveProductViewModel> Add(SaveProductViewModel vm)
        {
            throw new NotImplementedException();
        }

        public Task DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductViewModel>> GetAll()
        {
            var products = await _productRepository.GetAllWithIncludesAsync(new List<string> {"User", "productType", "transactions" });
            return _mapper.Map<List<ProductViewModel>>(products);
        }

        public Task<ProductViewModel> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(SaveProductViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}
