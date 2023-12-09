using EBazar_BAL.Interfaces;
using EBazar_BAL.Models;
using EBazar_DAL;
using EBazar_DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_BAL.Managers
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenHelper _tokenHelper;
        public static Random random = new Random();
        private readonly AppDbContext _context;
        public AuthManager(
            ITokenHelper tokenHelper,
            UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _tokenHelper = tokenHelper;
            _context = context;
        }
        public AuthManager(SignInManager<User> signInManager,
            ITokenHelper tokenHelper,
            UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHelper = tokenHelper;
            _context = context;
        }
        public AuthManager(){ }


        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
                return new LoginResult
                {
                    Success = false
                };

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

            if (result.Succeeded)
            {
                var token = await _tokenHelper.GenerateAccessToken(user);
                var refreshToken = _tokenHelper.CreateRefreshToken();

                user.RefreshToken = refreshToken;
                await _userManager.UpdateAsync(user);

                return new LoginResult
                {
                    Success = true,
                    AccessToken = token,
                    RefreshToken = refreshToken,
                    Username = user.UserName
                };
            }
            else
                return new LoginResult
                {
                    Success = false
                };
        }

        public async Task<int> Register(RegisterModel registerModel, string key)
        {
            var user = new User
            {
                Email = registerModel.Email,
                UserName = registerModel.UserName,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Age = registerModel.Age,
                ActivationKey = key
            };
            if (await _userManager.FindByEmailAsync(user.Email) != null)
            {
                return 2; //email already used
            }
            if (await _userManager.FindByNameAsync(user.UserName) != null)
            {
                return 3; //username already used
            }

            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, registerModel.Role);
                await _context.SaveChangesAsync();
                return 1;
            }

            return 0; //error while creating account

        }


        public async Task<string> Refresh(RefreshModel refreshModel)
        {
            var principal = _tokenHelper.GetPrincipalFromExpiredToken(refreshModel.AccessToken);
            var username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);

            if (user.RefreshToken != refreshModel.RefreshToken)
                return "Bad Refresh";

            var newJwtToken = await _tokenHelper.GenerateAccessToken(user);

            return newJwtToken;
        }
        public async Task<bool> ChangePassword(ChangePasswordModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }
    }
}
