using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class GenericService<SaveViewModel, ViewModel, Entity> : IGenericService<SaveViewModel, ViewModel, Entity> where SaveViewModel : class, IHasId where ViewModel : class where Entity : class
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

        public async Task<ViewModel> GetById(int id)
        {
            Entity entity  =await  _genericRepository.GetByIdAsync(id);

            ViewModel viewModel = _mapper.Map<ViewModel>(entity);

            return viewModel; 

        }

        public async Task Update(SaveViewModel viewModel)
        {

            Entity entity = await _genericRepository.GetByIdAsync(viewModel.Id);      
            entity = _mapper.Map(viewModel, entity);
            await _genericRepository.UpdateAsync(entity, viewModel.Id);
           
        }
    }
}
