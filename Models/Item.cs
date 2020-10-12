using System;
using System.ComponentModel.DataAnnotations;

namespace taleOfDungir.Models
{
    public class Item
    {
        public Int64 Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        public int Level { get; set; }
        public Rarity Rarity { get; set; }
        public int Value { get; set; }
        public int Power { get; set; }
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
}