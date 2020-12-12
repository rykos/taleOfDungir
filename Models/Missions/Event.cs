using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace taleOfDungir.Models
{
    public class Event
    {
        public Int64 Id { get; set; }

        public string ImageLocation { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        public List<EventAction> EventActions { get; set; }
    }
}