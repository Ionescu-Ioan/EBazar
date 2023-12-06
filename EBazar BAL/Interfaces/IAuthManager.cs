using EBazar_BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_BAL.Interfaces
{
    public interface IAuthManager
    {
        Task<int> Register(RegisterModel registerModel, string key);
        Task<LoginResult> Login(LoginModel loginModel);
        Task<string> Refresh(RefreshModel refreshModel);
        Task<bool> ChangePassword(ChangePasswordModel changePasswordModel);
    }
}
