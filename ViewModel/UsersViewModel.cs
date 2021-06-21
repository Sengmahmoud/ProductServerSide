using API.Dtos;
using API.Models.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModel
{
    public class UsersViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }
        public UserInputDto UserInput { get; set; }
        public UserRoleDto UserRole { get; set; }
        public IdentityRole Role { get; set; }
    }
}
