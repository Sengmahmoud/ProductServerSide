using API.Dtos;
using API.Helpers;
using API.Services;
using API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly AppSettings _appSettings;

        public UserController(ISecurityService securityService, IOptions<AppSettings> appSettings)
        {
            _securityService = securityService;
            _appSettings = appSettings.Value;
        }
        // GET: api/<UserController>/Users
       //// [Route("api/[controller]/Users")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            var model = new UsersViewModel
            {
                Users = _securityService.Users()
            };
            return Ok(model.Users.Select(u=>new {u.Id,u.UserName,u.PasswordHash }));

        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserInputDto userInput)
        {
            var user = await _securityService.Authenticate(userInput);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.Id,
                Username = user.UserName,
                Token = tokenString
            });
        }

        [Route("api/[controller]/Register")]
        // POST: api/<UserController>/Register
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserInputDto userInput )
        {


            await _securityService.Add(userInput);
            return Ok("added");
        }
        [Route("api/[controller]/Roles")]
        // GET: api/<UserController>/Roles
        [HttpGet]
        public IActionResult GetRoles()
        {
            var model = new UsersViewModel
            {
                Roles = _securityService.Roles()
            };
            return Ok(model);

        }
        //POST: api/<UserController>/AddRole
        [Route("api/[controller]/AddRole")]
        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] IdentityRole  Role)
        {
           await _securityService.AddRole(Role);
            return Ok("added");
        }
        //POST: api/<UserController>/AddUserToRole
        [Route("api/[controller]/AddUserToRole")]
        [HttpPost]
        public async Task<IActionResult> AddUserToRole([FromBody] UserRoleDto UserRole)
        {
           await _securityService.AddRoleToUser(UserRole);
            return Ok("added");
        }


    }
}
