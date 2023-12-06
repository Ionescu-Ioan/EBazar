using EBazar_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_BAL.Interfaces
{
    public interface ITokenHelper
    {
        Task<string> GenerateAccessToken(User _User);
        string CreateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string _Token);

    }
}
