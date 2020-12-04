using System;
using taleOfDungir.Models;

namespace taleOfDungir.Helpers
{
    public static class RarityHelper
    {
        static private Random random = new Random();
        public static Rarity RandomRarity()
        {
            int rnd = random.Next(1, 101);
            if (rnd < 50)
            {
                return Rarity.Common;
            }
            else if (rnd < 75)
            {
                return Rarity.Uncommon;
            }
            else if (rnd < 85)
            {
                return Rarity.Rare;
            }
            else if (rnd < 95)
            {
                return Rarity.Epic;
            }
            else
            {
                return Rarity.Legendary;
            }
        }
    }
}