using System;
using Microsoft.AspNetCore.Identity;
using taleOfDungir.Models;
using taleOfDungir.Data;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

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
            return character.Inventory.Select(i => new
            {
                i.ItemId,
                i.ItemType,
                i.Name,
                i.Level,
                i.Power,
                i.Value,
                iconID = i.ImageId,
                i.Worn,
                stats = SystemHelper.Deserialize<Stats>(i.Stats)
            }).ToList();
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

        public void EquipItem(long characterId, Item item)
        {
            if (item.ItemType == ItemType.Consumable || item.ItemType == ItemType.Resource || item.ItemType == ItemType.Trash)
                return;
            Character character = this.dbContext.Characters.Include(c => c.Inventory).Include(c => c.Skills).FirstOrDefault(c => c.CharacterId == characterId);
            Item alreadyWornItem = character.Inventory.FirstOrDefault(i => i.ItemType == item.ItemType && i.Worn);
            this.dbContext.Characters.Update(character);
            this.dbContext.Items.Update(item);

            item.Worn = true;
            //Unequiping active item
            if (alreadyWornItem != default)
            {
                this.dbContext.Update(alreadyWornItem);
                alreadyWornItem.Worn = false;
            }

            this.RecalculateCharacterStats(character);
            this.dbContext.SaveChanges();
        }

        public void RecalculateCharacterStats(Character character)
        {
            CharacterStats cs = new CharacterStats(character.Skills, this.EquippedItems(character));
            character.CharacterStats = SystemHelper.Serialize(cs);
        }

        private Item[] EquippedItems(Character character)
        {
            return character.Inventory.Where(i => i.Worn).ToArray();
        }

        public void SellItem(long characterId, Item item)
        {
            Character character = this.dbContext.Characters.Include(c => c.Inventory).FirstOrDefault(c => c.CharacterId == characterId);
            character.Gold += item.Value;
            this.dbContext.Remove(item);
            this.dbContext.Update(character);
            this.dbContext.SaveChanges();
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

        /// <returns>Enchance success</returns>
        public bool EnchanceSkill(long characterId, string skillName)
        {
            Character c = this.dbContext.Characters.Include(c => c.Skills).Include(c => c.Inventory).FirstOrDefault(c => c.CharacterId == characterId);
            bool success = false;
            if (skillName == "vitality")
            {
                if (this.Pay(c, this.EnchanceSkillPrice(c.Skills.Vitality)))
                {
                    c.Skills.Vitality++;
                    success = true;
                }
            }
            else if (skillName == "combat")
            {
                if (this.Pay(c, this.EnchanceSkillPrice(c.Skills.Combat)))
                {
                    c.Skills.Combat++;
                    success = true;
                }
            }
            else if (skillName == "luck")
            {
                if (this.Pay(c, this.EnchanceSkillPrice(c.Skills.Luck)))
                {
                    c.Skills.Luck++;
                    success = true;
                }
            }
            else if (skillName == "perception")
            {
                if (this.Pay(c, this.EnchanceSkillPrice(c.Skills.Perception)))
                {
                    c.Skills.Perception++;
                    success = true;
                }
            }
            if (success)
            {
                dbContext.Update(c);
                this.RecalculateCharacterStats(c);
                dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public long EnchanceSkillPrice(int skillLevel)
        {
            return (long)System.Math.Round((double)skillLevel * 1.5d);
        }

        private bool Pay(Character c, long amount)
        {
            if (c.Gold >= amount)
            {
                c.Gold -= amount;
                return true;
            }
            return false;
        }

        public void TakeDamage(Character character, long amount)
        {
            this.dbContext.Characters.Update(character);
            character.Health -= Math.Abs(amount);
            if (character.Health < 1)
                character.Health = 1;
            this.dbContext.SaveChanges();
        }

        public void TakeHealing(Character character, long amount)
        {
            this.dbContext.Update(character);
            character.Health += amount;
            if (character.Health > character.MaxHealth)
                character.Health = character.MaxHealth;
            this.dbContext.SaveChanges();
        }

        public void HealthRegen(Character character)
        {
            double passedMinutes = (DateTime.Now - character.LastHealthCheck).TotalMinutes;
            long regeneratedHealth = (long)Math.Round(passedMinutes * character.HealthRegen);
            if (regeneratedHealth == 0)
                return;

            character.LastHealthCheck = DateTime.Now;
            this.TakeHealing(character, regeneratedHealth);
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
        void TakeHealing(Character character, long amount);
        void HealthRegen(Character character);
        void EquipItem(long characterId, Item item);
        void SellItem(long characterId, Item item);
        /// <returns>Enchance success</returns>
        bool EnchanceSkill(long characterId, string skillName);
        long EnchanceSkillPrice(int skillLevel);
    }
}