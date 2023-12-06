using EBazar_DAL.Entities;
using EBazar_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<Boolean> removeUser(String username);
        Task<Boolean> emailExist(String email);
        Task<Boolean> usernameExist(String username);
        Task<UserModel> toUserModel(User userEntity);
        Task<UserModel> getUserByUsername(string username);
        Task<bool> ConfirmEmail(string email, string key);
    }
}
