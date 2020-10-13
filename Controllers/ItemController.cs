using Microsoft.AspNetCore.Mvc;
using taleOfDungir.Data;
using taleOfDungir.Models;
using System.Linq;
using System;

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

        private object ItemTransport(object item)
        {
            string description = "";
            if (item.GetType() == typeof(Weapon))
            {
                Weapon w = (Weapon)item;
                description = (item as Weapon).Description + $"\n{w.Power * 0.75}-{w.Power * 1.25}";
            }
            else if (item.GetType() == typeof(Armor))
            {
                Armor a = (Armor)item;
                description = (item as Armor).Description + $"\n{a.Power} defense";
            }

            object x = new
            {
                Name = ((Item)item).Name,
                Level = ((Item)item).Level,
                Rarity = ((Item)item).Rarity,
                Description = description
            };
            return x;
        }

        [HttpGet]
        [Route("weapon")]
        public IActionResult GetWeapons()
        {
            Weapon[] weapons = this.dbContext.Set<Weapon>().ToArray();
            return Ok(weapons);
        }

        [HttpGet]
        [Route("armor")]
        public IActionResult GetArmors()
        {
            Armor[] armors = this.dbContext.Set<Armor>().ToArray();
            return Ok(armors);
        }

        [HttpPost]
        [Route("weapon")]
        public IActionResult CreateWeapon([FromBody] Weapon weapon)
        {
            this.dbContext.Add(weapon);
            this.dbContext.SaveChanges();
            return Ok(weapon);
        }

        [HttpPost]
        [Route("armor")]
        public IActionResult CreateArmor([FromBody] Armor armor)
        {
            this.dbContext.Add(armor);
            this.dbContext.SaveChanges();
            return Ok(armor);
        }
    }
}