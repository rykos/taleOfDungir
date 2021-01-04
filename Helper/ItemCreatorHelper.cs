using System;
using System.Collections.Generic;
using System.Linq;
using taleOfDungir.Data;
using taleOfDungir.Models;

namespace taleOfDungir.Helpers
{
    public class ItemCreatorHelper : ItemCreatorHelperProvider
    {
        private readonly AppDbContext dbContext;
        private readonly NameHelperProvider nameHelper;
        public ItemCreatorHelper(AppDbContext dbContext, NameHelperProvider nameHelper)
        {
            this.dbContext = dbContext;
            this.nameHelper = nameHelper;
        }

        public Item CreateItem(int level)
        {
            ItemType itemType = GetRandomItemType();
            Rarity itemRarity = CreateItemRarity();
            Int64 power = (Int64)(this.GetPowerScale(itemRarity) * level);
            Item item = new Item()
            {
                Level = level,
                Name = this.nameHelper.GetNameFor(itemType),
                Description = "none",
                Rarity = itemRarity,
                Power = (Int64)(this.GetPowerScale(itemRarity) * level),
                Value = power * 2,
                ItemId = this.RandomItemImageID(itemType)
            };
            item.Stats = SystemHelper.Serialize(this.CreateStats(item));
            return item;
        }

        private double GetPowerScale(Rarity rarity)
        {
            double scale;
            switch (rarity)
            {
                case Rarity.Common:
                    scale = 1;
                    break;
                case Rarity.Uncommon:
                    scale = 1.25;
                    break;
                case Rarity.Rare:
                    scale = 1.5;
                    break;
                case Rarity.Epic:
                    scale = 2;
                    break;
                case Rarity.Legendary:
                    scale = 3;
                    break;
                default:
                    scale = 1;
                    break;
            }
            return scale;
        }

        private Rarity CreateItemRarity()
        {
            Rarity rarity;
            Random rnd = new Random();
            double roll = rnd.NextDouble() * 100;
            if (roll < 50)
            {
                rarity = Rarity.Common;
            }
            else if (roll < 75)
            {
                rarity = Rarity.Uncommon;
            }
            else if (roll < 90)
            {
                rarity = Rarity.Rare;
            }
            else if (roll < 97)
            {
                rarity = Rarity.Epic;
            }
            else
            {
                rarity = Rarity.Legendary;
            }
            return rarity;
        }

        private ItemType[] GetItemTypes()
        {
            return Enum.GetNames(typeof(ItemType)).Cast<ItemType>().ToArray();
        }

        private ItemType GetRandomItemType()
        {
            Array itemTypes = Enum.GetValues(typeof(ItemType));
            return (ItemType)itemTypes.GetValue(new Random().Next(0, itemTypes.Length));
        }

        private Stats CreateStats(Item item)
        {
            Stats stats = new Stats()
            {
                Combat = 1,
                Luck = 2,
                Perception = 2,
                Vitality = 3
            };
            //item.Stats = SystemHelper.Serialize(stats);
            return stats;
        }

        /// <summary>
        /// Returns random image id with specific itemType. (-1 if non existant)
        /// </summary>
        private long RandomItemImageID(ItemType itemType)
        {
            Random rnd = new Random();
            int amount = this.dbContext.ImageDBModels.Where(img => img.ItemType == itemType).Count();
            if (amount == 0)
            {
                DebugHelper.WriteError($"ERROR: NO IMAGE FOR ItemType:({itemType})");
                return -1;
            }
            long imgId = this.dbContext.ImageDBModels.Where(img => img.ItemType == itemType).Select(img => img.Id).Skip(rnd.Next(0, amount)).FirstOrDefault();
            return imgId;
        }
    }

    public interface ItemCreatorHelperProvider
    {
        Item CreateItem(int level);
    }
}