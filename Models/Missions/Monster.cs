using System;

namespace taleOfDungir.Models
{
    public class Monster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public Int64 Health { get; set; }
        public Int64 Damage { get; set; }
    }
}