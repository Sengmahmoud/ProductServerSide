using API.Dtos;
using API.Models.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
  public interface ISecurityService
    {
        IEnumerable<ApplicationUser> Users();
         Task<ApplicationUser> GetById(string id);
         Task Add(UserInputDto user);
        IEnumerable<IdentityRole> Roles();
        Task AddRole(IdentityRole role);
        Task AddRoleToUser(UserRoleDto userRole);
        Task<ApplicationUser> Authenticate(UserInputDto userInput);

    }
}
