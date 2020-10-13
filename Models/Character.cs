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

        public List<Item> Inventory { get; set; }//Items in bag
        public List<Item> Equipment { get; set; }//Equipped items

        public Stats Stats { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}