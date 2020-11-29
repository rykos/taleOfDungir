using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taleOfDungir.Models;
using taleOfDungir.Data;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections;

namespace taleOfDungir.Controllers
{
    /// <summary>
    /// Used to manage server variables, such as global items, event, difficulty
    /// </summary>
    [ApiController]
    [Route("admin")]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public AdminController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("items/names")]
        public IActionResult GetNames([FromQuery] string pattern)
        {
            ItemName[] itemNames;
            if (pattern == default)
            {
                itemNames = this.dbContext.ItemNames?.Take(10)?.ToArray();
            }
            else
            {
                itemNames = this.dbContext.ItemNames?.Where(i => i.Name.Contains(pattern))?.Take(10)?.ToArray();
            }
            return Ok(itemNames);
        }

        [HttpPost]
        [Route("items/names")]
        public IActionResult AddName([FromQuery] string itemType, [FromQuery] string itemName)
        {
            if (this.dbContext.ItemNames.FirstOrDefault(i => i.Name == itemName) != default)
            {
                return Ok(new Response(Models.Response.Error, "This item name already exists"));
            }
            ItemType newItemType;
            if (!Enum.TryParse<ItemType>(itemType, true, out newItemType))
            {
                return Ok(new Response(Models.Response.Error, "Invalid ItemType"));
            }
            this.dbContext.ItemNames.Add(new ItemName() { ItemType = newItemType, Name = itemName });
            this.dbContext.SaveChanges();
            return Ok(new Response(Models.Response.Success, null));
        }

        [HttpDelete]
        [Route("items/names/{id}")]
        public IActionResult RemoveName(int id)
        {
            ItemName itemName;
            if ((itemName = this.dbContext.ItemNames.FirstOrDefault(i => i.Id == id)) == default)
            {
                return Ok(new Response(Models.Response.Error, "Item with this ID does not exist"));
            }
            this.dbContext.ItemNames.Remove(itemName);
            this.dbContext.SaveChanges();
            return Ok();
        }
    }
}