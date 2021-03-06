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
using taleOfDungir.Data;
using Microsoft.EntityFrameworkCore;
using taleOfDungir.Helpers;
using System.Security.Cryptography;

namespace taleOfDungir.Controllers
{
    [ApiController]
    [Authorize]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private static string adminKey = GenerateAdminKey();
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly AppDbContext dbContext;
        private readonly CharacterHelperProvider characterHelper;
        private readonly ItemCreatorHelperProvider itemHelper;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IConfiguration configuration,
                                 AppDbContext dbContext,
                                 CharacterHelperProvider characterHelper,
                                 ItemCreatorHelperProvider itemHelper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.dbContext = dbContext;
            this.characterHelper = characterHelper;
            this.itemHelper = itemHelper;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            ApplicationUser user = await this.userManager.FindByNameAsync(loginModel.Username);
            if (user == default)
            {
                return Unauthorized(new Response("Error", "User does not exist"));
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
                    expiration = token.ValidTo,
                    admin = userRoles.Contains("admin")
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
                await this.CreateCharacter(newApplicationUser);
                return Ok(registerModel);
            }
            else
            {
                return Ok(new Response("Error", "User creation failed, try again later."));
            }
        }

        [HttpPost]
        [Route("register-admin")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin([FromBody] AdminRegisterModel registerModel)
        {
            if (registerModel.Key != adminKey)
            {
                Console.WriteLine(adminKey);
                return Unauthorized(new Response("Error", "Invalid key"));
            }
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

            if (await this.roleManager.RoleExistsAsync("admin") == false)
            {
                await this.roleManager.CreateAsync(new IdentityRole("admin"));
            }

            ApplicationUser newApplicationUser = new ApplicationUser()
            {
                UserName = registerModel.Username,
                Email = registerModel.Email
            };
            IdentityResult identityResult = await this.userManager.CreateAsync(newApplicationUser, registerModel.Password);
            if (identityResult.Succeeded)
            {
                await this.CreateCharacter(newApplicationUser);
                await userManager.AddToRoleAsync(newApplicationUser, "admin");
                adminKey = GenerateAdminKey();
                return Ok(registerModel);
            }
            else
            {
                return Ok(new Response("Error", "User creation failed, try again later."));
            }
        }

        [HttpPost]
        [Route("remove-account/{accountID}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RemoveAccount(string accountID)
        {
            ApplicationUser user = await this.userManager.FindByIdAsync(accountID);
            if (user == default)
            {
                return Ok(new Response(Models.Response.Error, $"User with id {accountID} does not exist"));
            }
            IdentityResult resoult = await this.userManager.DeleteAsync(user);
            if (resoult.Succeeded)
            {
                return Ok();
            }
            else
            {
                return Ok(new Response(Models.Response.Error, $"There was a problem deleting account with id {accountID}"));
            }
        }

        private async Task CreateCharacter(ApplicationUser user)
        {
            Character character = new Character()
            {
                Health = 100,
                MaxHealth = 100,
                Gold = 0,
                Exp = 0,
                Skills = new Skills(),
                LifeSkills = new LifeSkills()
            };
            character.ApplicationUserId = user.Id;
            this.dbContext.Characters.Add(character);
            this.dbContext.SaveChanges();

            user.CharacterId = character.CharacterId;
            await this.userManager.UpdateAsync(user);
        }

        private static string GenerateAdminKey()
        {
            string key;
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);

                key = Convert.ToBase64String(tokenData);
            }
            return key;
        }
    }
}