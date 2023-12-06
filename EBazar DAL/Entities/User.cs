using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_DAL.Entities
{
    public class User : IdentityUser<int>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string? RefreshToken { get; set; }
        public string ActivationKey { get; set; }


    }
}
