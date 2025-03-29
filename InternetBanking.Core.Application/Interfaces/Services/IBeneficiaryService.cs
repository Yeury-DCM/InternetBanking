using InternetBanking.Core.Application.ViewModels.BeneficiaryVMS;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IBeneficiaryService : IGenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>
    {
        Task AddBeneficiary(SaveBeneficiaryViewModel vm);
        Task DeleteBeneficiary(int id);
        Task<List<BeneficiaryViewModel>> GetAllBeneficiaries(string userId);
    }
}
