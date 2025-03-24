using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IGenericService<SaveViewModel, ViewModel, Entity> where SaveViewModel : class where ViewModel : class where Entity : class 
    {
        Task<SaveViewModel> Add(SaveViewModel vm);
        Task<List<ViewModel>> GetAll();
        Task<ViewModel> GetById(int id);
        Task Update (SaveViewModel viewModel);
        Task DeleteById(int id);
    }
}
