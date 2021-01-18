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

            this.characterHelper.HealthRegen(character);

            return Ok(new
            {
                level = character.Level,
                exp = character.Exp,
                reqExp = this.characterHelper.RequiredExp(character),
                gold = character.Gold,
                inventory = this.characterHelper.GetItemsDTO(character),
                lifeSkills = this.characterHelper.GetLifeSkillsDTO(character),
                skills = this.characterHelper.GetSkillsDTO(character),
                entity = new Entity(character)
            });
        }

        [HttpGet]
        [Route("equip/{itemID}")]
        public async Task<IActionResult> Equip(long itemID)
        {
            ApplicationUser activeUser = await this.GetActiveUser();
            Item item = this.dbContext.Items.FirstOrDefault(i => i.ItemId == itemID);
            if (activeUser == default)
                return Unauthorized();
            if (item.CharacterId != activeUser.CharacterId)
                return Unauthorized();

            Character character = this.dbContext.Characters.Include(c => c.Inventory).FirstOrDefault(c => c.CharacterId == activeUser.CharacterId);
            Item alreadyWornItem = character.Inventory.FirstOrDefault(i => i.ItemType == item.ItemType && i.Worn);
            if (alreadyWornItem != default)
            {
                this.dbContext.Update(alreadyWornItem);
                alreadyWornItem.Worn = false;
                if (alreadyWornItem.ItemId == itemID)
                {
                    this.dbContext.SaveChanges();
                    return Ok();
                }
            }

            this.dbContext.Update(item);
            item.Worn = true;
            this.dbContext.SaveChanges();

            return Ok();
        }

        private async Task<ApplicationUser> GetActiveUser()
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser applicationUser = await this.userManager.FindByIdAsync(userId);
            return applicationUser;
        }
    }
}