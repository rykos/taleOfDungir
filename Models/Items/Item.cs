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
        /// <summary>
        /// Value to merchants and crafting
        /// </summary>
        public Int64 Value { get; set; }
        /// <summary>
        /// Item main stat
        /// </summary>
        public Int64 Power { get; set; }

        /// <summary>
        /// Image ID in ImageDBModel
        /// </summary>
        public long ImageId { get; set; }

        public byte[] Stats { get; set; } = null;//Serialized stats structure

        public Character Character { get; set; }
        public Int64 CharacterId { get; set; }

        /// <summary>
        /// Is item equipped by player
        /// </summary>
        public bool Worn { get; set; } = false;

        public object ItemDTO()
        {
            return new
            {
                this.ItemId,
                this.Name,
                this.Description,
                this.Level,
                this.Power,
                this.Value,
                this.Worn,
                this.Rarity,
                this.Stats,
                this.ItemType
            };
        }
    }
}