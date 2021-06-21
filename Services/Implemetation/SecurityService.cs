using API.Dtos;
using API.Models.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Implemetation
{
    public class SecurityService : ISecurityService
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SecurityService(UserManager<ApplicationUser> userManger,RoleManager<IdentityRole> roleManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManger = userManger;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public async Task Add(UserInputDto user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var entity = new ApplicationUser
            {
                UserName = user.UserName,
                PasswordHash = user.Password
            };


            var result =await _userManger.CreateAsync(entity, user.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));
           
        }

        public async Task AddRole(IdentityRole role)
        {
            var result =await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));

        }

        public async Task AddRoleToUser(UserRoleDto userRole)
        {
            var user =await _userManger.FindByIdAsync(userRole.UserId);
          //  var roleId = _roleManager.FindByIdAsync(userRole.RoleId);
          
            
                var roleEntity =await _roleManager.FindByIdAsync(userRole.RoleId);
                var addResult = await _userManger.AddToRoleAsync(user, roleEntity.Name);
                if (!addResult.Succeeded)
                    throw new Exception(string.Join(",",
                        addResult.Errors.Select(e => e.Description)));
            
        }

        public async Task<ApplicationUser> Authenticate(UserInputDto userInput)
        {
            var result = await _signInManager.PasswordSignInAsync(userInput.UserName,userInput.Password,false,
                     lockoutOnFailure: false);

            if (!result.Succeeded)
                throw new Exception("error");

            var user = await _userManger.FindByNameAsync(userInput.UserName);
         
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return user;
            
        }

        public async Task<ApplicationUser> GetById(string id)
        {
          return await _userManger.FindByIdAsync(id);
        }

        public IEnumerable<IdentityRole> Roles()
        {
            return _roleManager.Roles.ToList();
        }

        public IEnumerable<ApplicationUser> Users()
        {
           return _userManger.Users.ToList();
        }

    }
}
