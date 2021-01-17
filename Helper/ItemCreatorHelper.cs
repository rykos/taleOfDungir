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
            return this.CreateItem(level, itemType);
        }

        public Item CreateItem(int level, ItemType itemType)
        {
            level = this.LevelVaration(level, 3);
            Rarity itemRarity = CreateItemRarity();
            Int64 power = (Int64)(this.GetPowerScale(itemRarity) * level);
            Item item = new Item()
            {
                Level = level,
                Name = this.nameHelper.NameFor(itemType),
                Description = "none",
                Rarity = itemRarity,
                Power = (Int64)(this.GetPowerScale(itemRarity) * level),
                Value = power * 2,
                ImageId = this.nameHelper.RandomItemImageIDFor(itemType),
                ItemType = itemType
            };
            item.Stats = SystemHelper.Serialize(this.CreateStats(item));
            return item;
        }

        private int LevelVaration(int level, int maxVariation)
        {
            Random rnd = new Random();
            int newLevel = level + (SystemHelper.RandomSign() * rnd.Next(0, maxVariation + 1));
            return newLevel < 1 ? 1 : newLevel;
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
            return (ItemType)itemTypes.GetValue(new Random().Next(1, itemTypes.Length));
        }

        private Stats CreateStats(Item item)
        {
            Random rnd = new Random();
            long availablePoints = (long)Math.Round(item.Level * 1.5f * this.GetPowerScale(item.Rarity));

            // Generate weight for each stat
            Stats stats = new Stats()
            {
                Combat = rnd.Next(1, 101),
                Luck = rnd.Next(1, 101),
                Perception = rnd.Next(1, 101),
                Vitality = rnd.Next(1, 101)
            };
            // Normalize stats to allocate available points
            stats.Normalize(availablePoints);

            return stats;
        }
    }

    public interface ItemCreatorHelperProvider
    {
        Item CreateItem(int level);
        Item CreateItem(int level, ItemType itemType);
    }
}