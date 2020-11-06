using System;
using System.ComponentModel.DataAnnotations;

namespace taleOfDungir.Models
{
    public class Item
    {
        public Int64 ItemId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        public int Level { get; set; }
        public ItemType ItemType { get; set; }
        public Rarity Rarity { get; set; }
        public Int64 Value { get; set; }
        public Int64 Power { get; set; }

        //
        public Character Character { get; set; }
        public Int64 CharacterId { get; set; }

        public bool Worn { get; set; } = false;
    }
}