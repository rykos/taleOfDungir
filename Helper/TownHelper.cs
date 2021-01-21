using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using taleOfDungir.Data;
using taleOfDungir.Models;

namespace taleOfDungir.Helpers
{
    public class TownHelper : TownHelperProvider
    {
        private readonly AppDbContext dbContext;
        private readonly ItemCreatorHelperProvider itemCreatorHelper;
        public TownHelper(AppDbContext dbContext, ItemCreatorHelperProvider itemCreatorHelper)
        {
            this.dbContext = dbContext;
            this.itemCreatorHelper = itemCreatorHelper;
        }

        public object[] GetBlacksmithItems(long characterId)
        {
            Item[] items = this.dbContext.Items.Where(i => i.CharacterId == characterId && i.Merchant == "blacksmith").ToArray();
            CharacterMetaData characterMetaData = this.dbContext.CharacterMetaDatas.FirstOrDefault(c => c.CharacterId == characterId);
            //No items | older than 1 minute
            if (items.Length == 0 || DateTime.Now > characterMetaData.BlackSmithItemGenerationTime.AddMinutes(1))
            {
                Character character = this.dbContext.Characters.Include(c => c.Inventory).FirstOrDefault(c => c.CharacterId == characterId);
                this.dbContext.Update(character);
                this.dbContext.Update(characterMetaData);
                //Remove all previous items from store
                character.Inventory.RemoveAll(i => i.Merchant == "blacksmith");
                //Create new items
                items = this.CreateItems(character.Level, 8, "blacksmith").ToArray();
                character.Inventory.AddRange(items);
                characterMetaData.BlackSmithItemGenerationTime = DateTime.Now;
                this.dbContext.SaveChanges();
            }
            return items.Select(i => i.ItemDTO()).ToArray();
        }

        private Item[] CreateItems(int level, int amount, string merchant)
        {
            Item[] items = new Item[amount];
            for (int i = 0; i < 8; i++)
            {
                items[i] = this.itemCreatorHelper.CreateItem(level);
                items[i].Merchant = merchant;
            }
            return items;
        }
    }

    public interface TownHelperProvider
    {
        object[] GetBlacksmithItems(long characterId);
    }
}