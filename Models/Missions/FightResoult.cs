using System.Collections.Generic;

namespace taleOfDungir.Models
{
    public class FightResoult
    {
        public bool Won { get; set; }
        public List<FightTurn> Turns { get; set; }

        public Entity Player { get; set; }
        public Entity Enemy { get; set; }

        public FightResoult(bool won, List<FightTurn> turns, Entity player, Entity enemy)
        {
            this.Won = won;
            this.Turns = turns;
            this.Player = player;
            this.Enemy = enemy;
        }
    }
}
// won = (character.Health > 0),
// turns = fightTurns,
// player = new
// {
//     health = character.Health
// },
// enemy = monster