using EBazar_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_BAL.Interfaces
{
    public interface IUserManager
    {
        Task<Boolean> removeUser(String username);
        Task<Boolean> emailExist(String email);
        Task<Boolean> usernameExist(String username);
        Task<bool> ConfirmEmail(string email, string key);
        Task<UserModel> getUserByUsername(string username);

    }
}
