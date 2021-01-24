using System;
using taleOfDungir.Helpers;

namespace taleOfDungir.Models
{
    /// <summary>
    /// Character stats are being recalculated on every character change
    /// </summary>
    [Serializable]
    public struct CharacterStats
    {
        public Stats FinalStats { get; set; }

        public CharacterStats(Skills skills, Item[] equipment)
        {
            this.FinalStats = new Stats(skills);
            foreach (Item item in equipment)
            {
                this.FinalStats = this.FinalStats + SystemHelper.Deserialize<Stats>(item.Stats);
            }
        }
    }
}