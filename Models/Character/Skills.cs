using System;

namespace taleOfDungir.Models
{
    public class Skills
    {
        public Int64 SkillsId { get; set; }

        public Character Character { get; set; }
        public Int64 CharacterId { get; set; }

        public int Vitality { get; set; } = 1;//Health
        public int Combat { get; set; } = 1;//Damage
        public int Luck { get; set; } = 1;//Better loot
        public int Perception { get; set; } = 1;//Chance to deal critical damage, and hit
    }
}