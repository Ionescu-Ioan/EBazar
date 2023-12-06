using EBazar_DAL.Entities;
using EBazar_DAL.Interfaces;
using EBazar_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Boolean> removeUser(String username)
        {
            var userEntity = await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
            if (userEntity == null)
            {
                return false; // nu exista acest user
            }
            _context.Remove(userEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> emailExist(string email)
        {
            var userEntity = await _context.Users.Where(x => x.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
            if (userEntity == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> usernameExist(string username)
        {
            var userEntity = await _context.Users.Where(x => x.UserName.ToLower() == username.ToLower()).FirstOrDefaultAsync();
            if (userEntity == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<UserModel> toUserModel(User userEntity)
        {
            var id = userEntity.Id;
            var role = await _context.UserRoles.Where(x => x.UserId == id).FirstOrDefaultAsync();
            string roleString;
            if (role.RoleId == 1)
            {
                roleString = "Admin";
            }
            else
            {
                roleString = "User";
            }
            var userModel = new UserModel
            {
                UserName = userEntity.UserName,
                Email = userEntity.Email,
                FirstName = userEntity.FirstName,
                UserRole = roleString,
                LastName = userEntity.LastName,
                Age = userEntity.Age

            };
            return userModel;
        }

        public async Task<UserModel> getUserByUsername(string username)
        {
            var userEntity = await _context.Users.Where(x => x.UserName.ToLower() == username.ToLower()).FirstOrDefaultAsync();
            if (userEntity != null)
            {
                var userReturn = await toUserModel(userEntity);
                return userReturn;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> ConfirmEmail(string email, string key)
        {
            var userEntity = await _context.Users.Where(x => x.Email.ToLower() == email).FirstOrDefaultAsync();
            if (userEntity != null)
            {
                if (key == userEntity.ActivationKey)
                {
                    userEntity.EmailConfirmed = true;
                    _context.Update(userEntity);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
