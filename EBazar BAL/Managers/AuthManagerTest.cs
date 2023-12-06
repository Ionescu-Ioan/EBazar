using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBazar_BAL.Models;
using NUnit.Framework;
using NUnit.Framework.Legacy;


namespace EBazar_BAL.Managers
{


    [TestFixture]
    public class AuthManagerTests
    {
        private readonly AuthManager _authManager;
        private readonly UserManager _userManager;
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsSuccessResult()
        {
            var user = new RegisterModel()
            {
                UserName = "username",
                Email = "testemail@gmail.com",
                Password = "Password01!",
                FirstName = "test",
                LastName = "test",
                Age = 12,
                Role = "User"
            };
            var key = "dfjfdids";
            var registerResult = await _authManager.Register(user, key);
            var readUser = await _userManager.getUserByUsername(user.UserName);
            Assert.That(registerResult==1, "Failed");
            Assert.That(readUser!=null, "Failed");
            }
        }

    
}
