using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using taleOfDungir.Models;

namespace taleOfDungir.Controllers
{
    [ApiController]
    [Authorize]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return Ok("Login function");
        }

        [HttpGet]
        [Route("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {
            return Ok(registerModel);
        }
    }
}