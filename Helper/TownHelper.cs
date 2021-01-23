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

        public MerchantStock GetBlacksmithStock(long characterId)
        {
            Item[] items = this.dbContext.Items.Where(i => i.CharacterId == characterId && i.Merchant == "blacksmith").ToArray();
            CharacterMetaData characterMetaData = this.dbContext.CharacterMetaDatas.FirstOrDefault(c => c.CharacterId == characterId);
            //No items | older than 1 minute
            if (items.Length == 0 || DateTime.Now > characterMetaData.BlackSmithItemGenerationTime.AddMinutes(1))
            {
                int level = this.dbContext.Characters.Select(c => new { c.CharacterId, c.Level }).FirstOrDefault(c => c.CharacterId == characterId).Level;
                //Remove all previous items from store
                if (items != default)
                    this.dbContext.Items.RemoveRange(items);
                //Create new items
                Item[] newItems = this.CreateItems(level, 8, "blacksmith", characterId).ToArray();
                characterMetaData.BlackSmithItemGenerationTime = DateTime.Now;

                this.dbContext.Items.AddRange(newItems);
                this.dbContext.CharacterMetaDatas.Update(characterMetaData);

                /// <summary>
                /// Suppress concurenccy exception
                /// </summary>
                try
                {
                    this.dbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException) { return null; }
                items = newItems;
            }
            return new MerchantStock()
            {
                Items = items.Select(i => i.ItemDTO()).ToArray(),
                RestockTime = characterMetaData.BlackSmithItemGenerationTime.AddMinutes(1)
            };
        }

        private Item[] CreateItems(int level, int amount, string merchant, long characterId)
        {
            Item[] items = new Item[amount];
            for (int i = 0; i < 8; i++)
            {
                items[i] = this.itemCreatorHelper.CreateItem(level);
                items[i].Merchant = merchant;
                items[i].CharacterId = characterId;
            }
            return items;
        }
    }

    public interface TownHelperProvider
    {
        MerchantStock GetBlacksmithStock(long characterId);
    }
}