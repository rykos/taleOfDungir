using System;

namespace taleOfDungir.Models
{
    public class CharacterMetaData
    {
        public long Id { get; set; }
        public long CharacterId { get; set; }
        public DateTime BlackSmithItemGenerationTime { get; set; }
    }
}