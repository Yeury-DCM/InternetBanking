using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class GenericService<SaveViewModel, ViewModel, Entity> : IGenericService<SaveViewModel, ViewModel, Entity> where SaveViewModel : class where ViewModel : class where Entity : class
    {
        private readonly IGenericRepository<Entity> _genericRepository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<Entity> genereciRepository, IMapper mapper)
        {
            _genericRepository = genereciRepository;
            _mapper = mapper;
        }

        public virtual async Task<SaveViewModel> Add(SaveViewModel viewModel)
        {

            Entity Entity = _mapper.Map<Entity>(viewModel);

            Entity = await _genericRepository.AddAsync(Entity);

            SaveViewModel EntityViewModel = _mapper.Map<SaveViewModel>(Entity);
            return EntityViewModel;

        }

        public async Task DeleteById(int id)
        {
            Entity entity = await  _genericRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new InvalidOperationException($"No hay ninguna entidad con el id: {id}");
            }
            await _genericRepository.DeleteAsync(entity);
        }

        public Task<List<ViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ViewModel> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(SaveViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}
