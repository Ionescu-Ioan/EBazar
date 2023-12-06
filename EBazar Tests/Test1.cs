using EBazar_BAL.Interfaces;
using EBazar_BAL.Managers;
using EBazar_DAL;
using EBazar_DAL.Entities;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;

namespace EBazar.UnitTests.Managers
{
    [TestFixture]
    public class AuthManagerTests
    {
        private IAuthManager _authManager;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<SignInManager<User>> _mockSignInManager;
        private Mock<ITokenHelper> _mockTokenHelper;
        private Mock<AppDbContext> _mockDbContext;

        [SetUp]
        public void SetUp()
        {
            _mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _mockSignInManager = new Mock<SignInManager<User>>(_mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null);
            _mockTokenHelper = new Mock<ITokenHelper>();
            _mockDbContext = new Mock<AppDbContext>();
            _authManager = new AuthManager(_mockSignInManager.Object, _mockTokenHelper.Object, _mockUserManager.Object, _mockDbContext.Object);
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsSuccessResult()
        {
            // Arrange
            var loginModel = new LoginModel { Email = "test@example.com", Password = "password" };
            var user = new User { Email = loginModel.Email, UserName = "TestUser" };
            _mockUserManager.Setup(u => u.FindByEmailAsync(loginModel.Email)).ReturnsAsync(user);
            _mockSignInManager.Setup(s => s.CheckPasswordSignInAsync(user, loginModel.Password, false)).ReturnsAsync(SignInResult.Success);
            _mockTokenHelper.Setup(t => t.GenerateAccessToken(user)).ReturnsAsync("fakeAccessToken");
            _mockTokenHelper.Setup(t => t.CreateRefreshToken()).Returns("fakeRefreshToken");

            // Act
            var result = await _authManager.Login(loginModel);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("fakeAccessToken", result.AccessToken);
            Assert.AreEqual("fakeRefreshToken", result.RefreshToken);
            Assert.AreEqual("TestUser", result.Username);
        }

        [Test]
        public async Task Register_ValidModel_ReturnsSuccessCode()
        {
            // Arrange
            var registerModel = new RegisterModel
            {
                Email = "newuser@example.com",
                UserName = "NewUser",
                FirstName = "John",
                LastName = "Doe",
                Age = 25,
                Password = "newpassword",
                Role = "User"
            };
            _mockUserManager.Setup(u => u.FindByEmailAsync(registerModel.Email)).ReturnsAsync((User)null);
            _mockUserManager.Setup(u => u.FindByNameAsync(registerModel.UserName)).ReturnsAsync((User)null);
            _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<User>(), registerModel.Password)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authManager.Register(registerModel, "activationKey");

            // Assert
            Assert.AreEqual(1, result);
        }
    }
}
