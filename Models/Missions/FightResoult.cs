using System.Collections.Generic;

namespace taleOfDungir.Models
{
    public class FightResoult
    {
        public bool Won;
        public List<FightTurn> Turns;
        public long PlayerHealth;
        public long EnemyHealth;

        public FightResoult(bool won, List<FightTurn> turns, long playerHealth, long enemyHealth)
        {
            Won = won;
            Turns = turns;
            PlayerHealth = playerHealth;
            EnemyHealth = enemyHealth;
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