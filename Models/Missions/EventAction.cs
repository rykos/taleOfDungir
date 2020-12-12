using System;
using System.ComponentModel.DataAnnotations;

namespace taleOfDungir.Models
{
    /// <summary>
    /// Action that you can perform in event
    /// </summary>
    public class EventAction
    {
        public Int64 Id { get; set; }

        [MaxLength(30)]
        public string Text { get; set; }

        [MaxLength(20)]
        public string SkillName { get; set; }

        public Int64 EventId { get; set; }
    }
}