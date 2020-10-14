using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace taleOfDungir.Models
{
    public class Character
    {
        public Int64 CharacterId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Gold { get; set; } = 0;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Level { get; set; } = 1;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Exp { get; set; } = 0;

        //Stats, WORK_IN_PROGRESS
        public Int64 Health { get; set; }
        //

        public List<Item> Inventory { get; set; }//Items in bag
        public List<Item> Equipment { get; set; }//Equipped items

        public Skills Skills { get; set; }//Combat skills
        public LifeSkills LifeSkills { get; set; }//Event skills

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}