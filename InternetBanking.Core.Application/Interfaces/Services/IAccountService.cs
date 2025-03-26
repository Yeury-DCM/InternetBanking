using InternetBanking.Core.Application.Dtos;
using InternetBanking.Core.Application.ViewModels.UserVMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<CreateUserResponse> CreateUser(CreateUserRequest request);
        Task SignOutAsync();

        Task<List<UserViewModel>> GetAllUserViewModelsAsync();
    }
}
