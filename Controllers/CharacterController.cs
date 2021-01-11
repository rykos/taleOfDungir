using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taleOfDungir.Data;
using taleOfDungir.Helpers;
using taleOfDungir.Models;

namespace taleOfDungir.Controllers
{
    [ApiController]
    [Authorize]
    [Route("character")]
    public class CharacterController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly CharacterHelperProvider characterHelper;
        private readonly AppDbContext dbContext;
        private readonly ItemCreatorHelperProvider itemCreatorHelper;
        public CharacterController(UserManager<ApplicationUser> userManager,
                                   CharacterHelperProvider characterHelper,
                                   AppDbContext dbContext,
                                   ItemCreatorHelperProvider itemCreatorHelper)
        {
            this.userManager = userManager;
            this.characterHelper = characterHelper;
            this.dbContext = dbContext;
            this.itemCreatorHelper = itemCreatorHelper;
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

            Character character = (this.userManager.Users
                .Include(u => u.Character)
                    .ThenInclude(c => c.Skills)
                .Include(u => u.Character)
                    .ThenInclude(c => c.LifeSkills)
                .Include(u => u.Character.Inventory)
                .Where(u => u.Id == userId).Select(u => u.Character)).FirstOrDefault();

            return Ok(new
            {
                avatarId = character.CharacterAvatarId,
                level = character.Level,
                exp = character.Exp,
                reqExp = this.characterHelper.RequiredExp(character),
                health = character.Health,
                gold = character.Gold,
                inventory = this.characterHelper.GetItemsDTO(character),
                lifeSkills = this.characterHelper.GetLifeSkillsDTO(character),
                skills = this.characterHelper.GetSkillsDTO(character)
            });
        }

        [HttpGet]
        [Route("give")]
        public IActionResult Give()
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser applicationUser = this.dbContext.Users.Include(u => u.Character.Inventory).Include(u => u.Character).FirstOrDefault(u => u.Id == userId);
            if (applicationUser == default)
            {
                return Unauthorized("User does not exist");
            }
            
            this.dbContext.Update(applicationUser);
            System.Random rnd = new System.Random();
            Item newItem = this.itemCreatorHelper.CreateItem(rnd.Next(0, 2) + applicationUser.Character.Level);
            newItem.CharacterId = applicationUser.CharacterId;

            applicationUser.Character.Inventory.Add(newItem);

            this.dbContext.SaveChanges();

            return Ok();
        }
    }
}