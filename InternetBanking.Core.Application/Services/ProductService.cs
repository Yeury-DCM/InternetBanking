using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.ProductVMS;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class ProductService : GenericService<SaveProductViewModel, ProductViewModel, Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper, IGenericRepository<Product> genericRepository) : base(genericRepository, mapper)
        {

            _productRepository = productRepository;
            _mapper = mapper;
        }


        public async Task<List<ProductViewModel>> GetAll()
        {
            var products = await _productRepository.GetAllWithIncludesAsync(new List<string> {"User", "productType", "transactions" });
            return _mapper.Map<List<ProductViewModel>>(products);
        }

    }
}
