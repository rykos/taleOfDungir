using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using taleOfDungir.Models;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace taleOfDungir.Controllers
{
    [ApiController]
    [Authorize]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            ApplicationUser user = await this.userManager.FindByNameAsync(loginModel.Username);
            if (user == default)
            {
                return Ok(new Response("Error", "User does not exist"));
            }
            bool passwordCheckResoult = await this.userManager.CheckPasswordAsync(user, loginModel.Password);
            if (passwordCheckResoult)
            {
                IList<string> userRoles = await this.userManager.GetRolesAsync(user);

                List<Claim> authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (string role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JWT:Secret"]));

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: this.configuration["JWT:ValidIssuer"],
                    audience: this.configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(4),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    id = user.Id,
                    username = user.UserName,
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            else
            {
                return Unauthorized(new Response("Error", "Wrong username or password"));
            }
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            ApplicationUser userByName = await this.userManager.FindByNameAsync(registerModel.Username);
            ApplicationUser userByEmail = await this.userManager.FindByEmailAsync(registerModel.Email);
            if (userByName != default)
            {
                return BadRequest("Username is taken");
            }
            if (userByEmail != default)
            {
                return BadRequest("Email is taken");
            }

            ApplicationUser newApplicationUser = new ApplicationUser()
            {
                UserName = registerModel.Username,
                Email = registerModel.Email
            };
            IdentityResult identityResult = await this.userManager.CreateAsync(newApplicationUser, registerModel.Password);
            if (identityResult.Succeeded)
            {
                return Ok(registerModel);
            }
            else
            {
                return Ok(new Response("Error", "User creation failed, try again later."));
            }
        }

        [HttpGet]
        [Route("details")]
        public async Task<IActionResult> Details()
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser applicationUser = await this.userManager.FindByIdAsync(userId);
            if (applicationUser == default)
            {
                return Unauthorized("Account does not exist");
            }

            return Ok(new
            {
                id = userId,
                name = applicationUser.UserName
            });
        }
    }
}