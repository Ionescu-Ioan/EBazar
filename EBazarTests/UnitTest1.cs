using EBazar_BAL.Interfaces;
using EBazar_BAL.Managers;
using EBazar_BAL.Models;
using EBazar_DAL;
using EBazar_DAL.Entities;
using EBazar_DAL.Interfaces;
using EBazar_DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazarTests
{


    [TestFixture]
    public class RegisterTest
    {
        private UserManager _userManager;

        private UserRepository _userRepository;
        private AppDbContext _contextMock;
        [SetUp]
        public void Setup()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Data Source=LAPTOP-MO2LLB6V;Initial Catalog=EbazarDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")  // Replace "your_connection_string_here" with your actual connection string
                .Options;

            _contextMock = new AppDbContext(dbContextOptions);
            _userRepository = new UserRepository(_contextMock);

            _userManager = new UserManager(_userRepository);
        }

        [Test, Order(1)]
        public async Task GetUserFromDatabase()
        {
            var user = new RegisterModel()
            {
                UserName = "rinualex",
                Email = "rinualexandru3@gmail.com",
                Password = "Password01!",
                FirstName = "test",
                LastName = "test",
                Age = 12,
                Role = "User"
            };
            var readUser = await _userManager.getUserByUsername(user.UserName);
            var emailExist = await _userManager.emailExist(user.Email);
            var usernameExist = await _userManager.usernameExist(user.UserName);

            Assert.IsNotNull(readUser != null, "UserIsNull!");
            if (readUser != null)
            {
                Assert.IsTrue(readUser.UserName == readUser.UserName, "UserName does not match!");
                Assert.IsTrue(usernameExist, "UserName does not exist!");
                Assert.IsTrue(emailExist, "Email does not exist!");
            }

        }

        [Test, Order(2)]
        public async Task RemoveUserFromDatabase()
        {
            var user = new RegisterModel()
            {
                UserName = "rinualex",
                Email = "rinualexandru3@gmail.com",
                Password = "Password01!",
                FirstName = "test",
                LastName = "test",
                Age = 12,
                Role = "User"
            };
            var remove = await _userManager.removeUser(user.UserName);
            Assert.IsTrue(remove, "User was not removed!");
            if (remove)
            {
                var readUser = await _userManager.getUserByUsername(user.UserName);
                Assert.IsNotNull(readUser == null, "UserName exists in application after it was removed!");

            }
        }
    }


}
