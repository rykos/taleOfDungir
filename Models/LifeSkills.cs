using System;

namespace taleOfDungir.Models
{
    public class LifeSkills
    {
        public Int64 LifeSkillsId { get; set; }

        public int Crafting { get; set; }
        public int Scavanging { get; set; }
        public int Dialog { get; set; }

        //
        public Character Character { get; set; }
        public Int64 CharacterId { get; set; }
    }
}