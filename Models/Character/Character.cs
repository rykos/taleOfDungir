using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace taleOfDungir.Models
{
    public class Character
    {
        public Int64 CharacterId { get; set; }

        public Int64 Gold { get; set; }
        public int Level { get; set; }
        public Int64 Exp { get; set; }

        //Stats, WORK_IN_PROGRESS
        public Int64 Health { get; set; }
        public Int64 Damage { get; set; }
        //

        public List<Item> Inventory { get; set; }//Items in bag

        public Skills Skills { get; set; }//Combat skills
        public LifeSkills LifeSkills { get; set; }//Event skills

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public List<Mission> Missions { get; set; }
    }
}