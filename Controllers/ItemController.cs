using Microsoft.AspNetCore.Mvc;
using taleOfDungir.Data;
using taleOfDungir.Models;
using System.Linq;
using System;
using System.Collections.Generic;

namespace taleOfDungir.Controllers
{
    [ApiController]
    [Route("item")]
    public class ItemController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public ItemController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetItem(int id)
        {
            object item = this.dbContext.Set<Item>().FirstOrDefault(x => x.ItemId == id);
            object resoult = this.ItemTransport(item);
            return Ok(resoult);
        }

        private object ItemTransport(object itemObject)
        {
            Item item = (Item)itemObject;
            object x = new
            {
                Name = item.Name,
                Level = item.Level,
                Rarity = item.Rarity,
                Description = item.Description
            };
            return x;
        }

        [HttpGet]
        [Route("weapon")]
        public IActionResult GetWeapons()
        {
            List<Item> weapons = this.dbContext.Items.Where(i => i.ItemType == ItemType.Weapon).ToList();
            return Ok(weapons);
        }

        [HttpPost]
        [Route("weapon")]
        public IActionResult CreateWeapon([FromBody] Item weapon)
        {
            this.dbContext.Add(weapon);
            this.dbContext.SaveChanges();
            return Ok(weapon);
        }

        [HttpPost]
        [Route("armor")]
        public IActionResult CreateArmor([FromBody] Item armor)
        {
            this.dbContext.Items.Add(armor);
            this.dbContext.SaveChanges();
            return Ok(armor);
        }
    }
}