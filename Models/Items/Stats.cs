using System;

namespace taleOfDungir.Models
{
    [Serializable]
    public struct Stats
    {
        public Stats(int vitality, int combat, int luck, int perception)
        {
            Vitality = vitality;
            Combat = combat;
            Luck = luck;
            Perception = perception;
        }

        public Stats(Skills skills)
        {
            this.Vitality = skills.Vitality;
            this.Combat = skills.Combat;
            this.Luck = skills.Luck;
            this.Perception = skills.Perception;
        }

        public static Stats operator +(Stats a, Stats b)
        {
            return new Stats(a.Vitality + b.Vitality,
             a.Combat + b.Combat,
             a.Luck + b.Luck,
             a.Perception + b.Perception);
        }

        public int Vitality { get; set; }
        public int Combat { get; set; }
        public int Luck { get; set; }
        public int Perception { get; set; }

        /// <summary>
        /// Normalize stats to fit in available points
        /// </summary>
        public void Normalize(long maxPoints)
        {
            double scale = (double)maxPoints / (this.Vitality + this.Combat + this.Luck + this.Perception);
            this.Vitality = (int)Math.Round(this.Vitality * scale);
            this.Combat = (int)Math.Round(this.Combat * scale);
            this.Luck = (int)Math.Round(this.Luck * scale);
            this.Perception = (int)Math.Round(this.Perception * scale);
        }
    }
}