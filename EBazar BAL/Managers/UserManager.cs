using EBazar_BAL.Interfaces;
using EBazar_DAL.Interfaces;
using EBazar_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_BAL.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepo;
        public UserManager(IUserRepository userRepo) :base()
        {
            _userRepo = userRepo;
        }

        public async Task<Boolean> removeUser(string username)
        {
            return await _userRepo.removeUser(username);
        }

        public async Task<bool> emailExist(string email)
        {
            return await _userRepo.emailExist(email);
        }

        public async Task<bool> usernameExist(string username)
        {
            return await _userRepo.usernameExist(username);
        }

        public async Task<bool> ConfirmEmail(string email, string key)
        {
            return await _userRepo.ConfirmEmail(email, key);
        }

        public async Task<UserModel> getUserByUsername(string username)
        {
            return await _userRepo.getUserByUsername(username);
        }
    }
}
