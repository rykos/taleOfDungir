using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using taleOfDungir.Data;
using taleOfDungir.Helpers;
using taleOfDungir.Models;

namespace taleOfDungir.Controllers
{
    [ApiController]
    [Route("town")]
    public class TownController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ItemCreatorHelperProvider itemCreatorHelper;
        private readonly CharacterHelperProvider characterHelper;
        private readonly AppDbContext dbContext;
        private readonly TownHelperProvider townHelper;
        public TownController(UserManager<ApplicationUser> userManager,
                              ItemCreatorHelperProvider itemCreatorHelper,
                              CharacterHelperProvider characterHelper,
                              AppDbContext dbContext,
                              TownHelperProvider townHelper)
        {
            this.userManager = userManager;
            this.itemCreatorHelper = itemCreatorHelper;
            this.characterHelper = characterHelper;
            this.dbContext = dbContext;
            this.townHelper = townHelper;
        }

        [HttpGet]
        [Route("sell/{itemID}")]
        public async Task<IActionResult> Sell(long itemID)
        {
            ApplicationUser activeUser = await this.GetActiveUser();
            Item item = this.dbContext.Items.FirstOrDefault(i => i.ItemId == itemID);
            if (item == default)
                return NotFound();
            if (activeUser == default)
                return Unauthorized();
            if (item.CharacterId != activeUser.CharacterId)
                return Unauthorized();

            this.characterHelper.SellItem(activeUser.CharacterId, item);
            return Ok();
        }

        /// <summary>
        /// Returns list of items available in shop
        /// </summary>
        [HttpGet]
        [Route("blacksmith/items")]
        public async Task<IActionResult> BlacksmithItems()
        {
            ApplicationUser user = await this.GetActiveUser();
            if (user == default)
                return Unauthorized();

            return Ok(this.townHelper.GetBlacksmithItems(user.CharacterId));
        }

        private async Task<ApplicationUser> GetActiveUser()
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser applicationUser = await this.userManager.FindByIdAsync(userId);
            return applicationUser;
        }
    }
}