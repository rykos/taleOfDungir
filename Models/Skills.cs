using System;

namespace taleOfDungir.Models
{
    public class Skills
    {
        public Int64 SkillsId { get; set; }

        public Character Character { get; set; }
        public Int64 CharacterId { get; set; }

        public int Vitality { get; set; }//Health
        public int Combat { get; set; }//Damage
        public int Luck { get; set; }//Better loot
        public int Perception { get; set; }//Chance to deal critical damage, and hit
    }
}