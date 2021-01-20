using System;
using System.Collections.Generic;
using System.Linq;
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

        public Item[] GetBlacksmithItems(long characterId)
        {
            Item[] items = this.dbContext.Items.Where(i => i.CharacterId == characterId && i.Merchant == "blacksmith").ToArray();
            CharacterMetaData characterMetaData = this.dbContext.CharacterMetaDatas.FirstOrDefault(c => c.CharacterId == characterId);
            //No items | older than 1 minute
            if (items.Length == 0 || characterMetaData.BlackSmithItemGenerationTime < (DateTime.Now.AddMinutes(1)))
            {
                Character c = this.dbContext.Characters.FirstOrDefault(c => c.CharacterId == characterId);
                this.dbContext.Update(c);
                items = this.CreateItems(c.Level, 8, "blacksmith").ToArray();
                c.Inventory.AddRange(items);
                this.dbContext.SaveChanges();
            }
            return items;
        }

        private Item[] CreateItems(int level, int amount, string merchant)
        {
            Item[] items = new Item[amount];
            for (int i = 0; i < 8; i++)
            {
                items[i] = this.itemCreatorHelper.CreateItem(level);
            }
            return items;
        }
    }

    public interface TownHelperProvider
    {
        Item[] GetBlacksmithItems(long characterId);
    }
}