using System;

namespace taleOfDungir.Models
{
    public class Mission
    {
        public int Id { get; set; }
        public Character Character { get; set; }
        public Int64 CharacterId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Rarity determinates difficulty and quality of rewards
        /// </summary>
        public Rarity Rarity { get; set; }
        /// <summary>
        /// Raw duration of mission in seconds
        /// </summary>
        public int Duration { get; set; }
    }
}