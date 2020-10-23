using System;
using System.Collections.Generic;
using System.Linq;
using taleOfDungir.Data;
using taleOfDungir.Models;

namespace taleOfDungir.Helpers
{
    public class ItemHelper : ItemHelperProvider
    {
        private readonly AppDbContext dbContext;
        private readonly NameHelperProvider nameHelper;
        public ItemHelper(AppDbContext dbContext, NameHelperProvider nameHelper)
        {
            this.dbContext = dbContext;
            this.nameHelper = nameHelper;
        }

        public Item CreateItem(int level)
        {
            //ItemType itemType = GetRandomItemType();
            ItemType itemType = ItemType.Head;
            Rarity itemRarity = CreateItemRarity();
            Int64 power = (Int64)(this.GetPowerScale(itemRarity) * level);
            return new Item()
            {
                Level = level,
                Name = this.nameHelper.GetNameFor(itemType),
                Description = "",
                Rarity = itemRarity,
                Power = (Int64)(this.GetPowerScale(itemRarity) * level),
                Value = power * 2
            };
        }

        public Item CreateItem(int level, ItemType itemType)
        {
            return new Item()
            {

            };
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
            ItemType[] itemTypes = this.GetItemTypes();
            return itemTypes[new Random().Next(0, itemTypes.Length)];
        }
    }

    public interface ItemHelperProvider
    {
        Item CreateItem(int level);
    }
}