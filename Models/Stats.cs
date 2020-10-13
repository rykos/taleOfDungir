using System;

namespace taleOfDungir.Models
{
    public class Stats
    {
        public Int64 StatsId { get; set; }

        public Character Character { get; set; }
        public Int64 CharacterId { get; set; }

        public int Vitality { get; set; }//Health
        public int Combat { get; set; }//Damage
        public int Luck { get; set; }//Better loot
        public int Perception { get; set; }//Chance to find additional events
    }
}