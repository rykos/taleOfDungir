using System;
using Microsoft.AspNetCore.Identity;
using taleOfDungir.Models;
using taleOfDungir.Data;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace taleOfDungir.Helpers
{
    //Helper for managing Character in database
    public class CharacterHelper : CharacterHelperProvider
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext dbContext;
        private readonly ItemCreatorHelperProvider itemCreatorHelper;
        public CharacterHelper(UserManager<ApplicationUser> userManager, AppDbContext dbContext, ItemCreatorHelperProvider itemCreatorHelper)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.itemCreatorHelper = itemCreatorHelper;
        }

        public void AddExp(Character character, Int64 amount)
        {
            this.dbContext.Characters.Update(character);
            character.Exp += amount;
            while (character.Exp >= character.Level * 100)//While character is elgible for level up
            {
                character.Exp -= character.Level * 100;
                character.Level++;
            }
            this.dbContext.SaveChanges();
        }

        public void AddGold(Character character, long amount)
        {
            this.dbContext.Characters.Update(character);
            character.Gold += amount;
            this.dbContext.SaveChanges();
        }

        /// <returns>Array of DataTransferObjects</returns>
        public object GetItemsDTO(Character character)
        {
            return character.Inventory.Select(i => new { i.Name, i.Level, i.Power, i.Value, iconID = i.ImageId }).ToList();
        }

        public object GetSkillsDTO(Character character)
        {
            return new
            {
                Combat = character.Skills.Combat,
                Luck = character.Skills.Luck,
                Perception = character.Skills.Perception,
                Vitality = character.Skills.Vitality
            };
        }

        public object GetLifeSkillsDTO(Character character)
        {
            return new
            {
                vitality = character.LifeSkills.Crafting,
                Dialog = character.LifeSkills.Dialog,
                Scavanging = character.LifeSkills.Scavanging
            };
        }

        public long RequiredExp(Character character)
        {
            return character.Level * 100;
        }

        public Item SpawnRandomItem(Character character)
        {
            //If inventory reference is missing, load it
            if (character.Inventory == null)
                character.Inventory = this.dbContext.Characters.Include(c => c.Inventory)
                    .Select(c => new { c.CharacterId, c.Inventory }).FirstOrDefault(c => c.CharacterId == character.CharacterId).Inventory;

            this.dbContext.Update(character);
            Item item = this.itemCreatorHelper.CreateItem(character.Level);
            character.Inventory.Add(item);
            this.dbContext.SaveChanges();
            return item;
        }

        public void TakeDamage(Character character, Int64 amount)
        {
            this.dbContext.Characters.Update(character);
            character.Health -= Math.Abs(amount);
            if (character.Health < 1)
                character.Health = 1;
            this.dbContext.SaveChanges();
        }
    }

    public interface CharacterHelperProvider
    {
        void AddExp(Character character, Int64 amount);
        void AddGold(Character character, Int64 amount);
        object GetItemsDTO(Character character);
        object GetSkillsDTO(Character character);
        object GetLifeSkillsDTO(Character character);
        long RequiredExp(Character character);
        Item SpawnRandomItem(Character character);
        void TakeDamage(Character character, Int64 amount);
    }
}