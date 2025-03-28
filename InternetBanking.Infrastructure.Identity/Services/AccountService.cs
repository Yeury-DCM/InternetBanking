
using Azure;
using InternetBanking.Core.Application.Dtos;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.UserVMS;
using InternetBanking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new() { IsSucess = true };

            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                response.IsSucess = false;
                response.ErrorMessage = "Credenciales inválidas";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);

            if (!result.Succeeded)
            {
                response.IsSucess = false;
                response.ErrorMessage = "Credenciales inválidas";
                return response;
            }

            response.Id = user.Id;
            response.Email = user.Email!;
            response.UserName = user.UserName!;

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<SaveUserResponse> CreateUser(SaveUserRequest request)
        {
           
            SaveUserResponse response = new() { IsSucess = true };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);

            if (userWithSameUserName != null)
            {
                response.IsSucess = false;
                response.ErrorMessage = $"El nombre de usuario '{request.UserName}' ya está tomado.";
                return response;
            }

            var userWithSameUserEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameUserEmail != null)
            {
                response.IsSucess = false;
                response.ErrorMessage = $"El correo '{request.Email}' ya está tomado.";
                return response;
            }

            var user = new ApplicationUser
            {
                Email = request.Email.Trim(),
                UserName = request.UserName.Trim(),
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                IdentificationNumber = request.IdentificationNumber

            };

            var result = await _userManager.CreateAsync(user, request.Password);

            await _userManager.AddToRoleAsync(user, request.Role.ToString());
                
            if (result.Succeeded)
            {
                response.UserId = user.Id;

            }
            else
            {
                response.IsSucess = false;
                response.ErrorMessage = $"Ocurrió un error iniciando sesión";
                return response;
            }

            return response;
        }

        public async Task<List<UserViewModel>> GetAllUserViewModelsAsync()
        {
            var users = await _userManager.Users.ToListAsync(); // Obtiene los usuarios primero

            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userViewModels.Add(new UserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IdentificationNumber = user.IdentificationNumber,
                    UserName = user.UserName!,
                    Email = user.Email,
                    Roles = roles.ToList(),
                    IsActive = user.IsActive
                });
            }

            return userViewModels.ToList();
        }

        public async Task<UserViewModel> GetUserViewModelByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            UserViewModel userViewModel = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                IdentificationNumber = user.IdentificationNumber,
                UserName = user.UserName!,
                Email = user.Email,
                Roles = (List<string>)await _userManager.GetRolesAsync(user),
                IsActive = user.IsActive
            };

            return userViewModel;
        }


    }
}
