using EBazar_DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_DAL
{
    public class InitialSeed
    {
        private readonly RoleManager<Role> _roleManager;
        public InitialSeed(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public void CreateRoles()
        {
            string[] roleNames =
            {
                "Admin",
                "User"
            };

            foreach (var name in roleNames)
            {
                var role = new Role
                {
                    Name = name
                };

                _roleManager.CreateAsync(role).Wait();
            }
        }
    }
}
