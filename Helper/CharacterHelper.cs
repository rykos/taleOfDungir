using System;
using Microsoft.AspNetCore.Identity;
using taleOfDungir.Models;
using taleOfDungir.Data;

namespace taleOfDungir.Helpers
{
    //Helper for managing Character in database
    public class CharacterHelper : CharacterHelperProvider
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext dbContext;
        public CharacterHelper(UserManager<ApplicationUser> userManager, AppDbContext dbContext)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
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

        public void TakeDamage(Character character, Int64 amount)
        {
            this.dbContext.Characters.Update(character);
            character.Health -= Math.Abs(amount);
            if (character.Health < 1)
            {
                character.Health = 1;
            }
            this.dbContext.SaveChanges();
        }
    }

    public interface CharacterHelperProvider
    {
        void AddExp(Character character, Int64 amount);
    }
}