using System;

namespace taleOfDungir.Models
{
    [Serializable]
    public struct Stats
    {
        public int Vitality { get; set; }
        public int Combat { get; set; }
        public int Luck { get; set; }
        public int Perception { get; set; }

        /// <summary>
        /// Normalize stats to fit in available points
        /// </summary>
        public void Normalize(long maxPoints)
        {
            double scale = (double)100 / (this.Vitality + this.Combat + this.Luck + this.Perception);
            this.Vitality = (int)Math.Round(this.Vitality * scale);
            this.Combat = (int)Math.Round(this.Combat * scale);
            this.Luck = (int)Math.Round(this.Luck * scale);
            this.Perception = (int)Math.Round(this.Perception * scale);
        }
    }
}