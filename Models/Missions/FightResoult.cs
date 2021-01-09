using System.Collections.Generic;

namespace taleOfDungir.Models
{
    public class FightResoult
    {
        public bool Won { get; set; }
        public List<FightTurn> Turns { get; set; }
        public long PlayerHealth { get; set; }
        public long EnemyHealth { get; set; }
        public long PlayerAvatarId { get; set; }
        public long EnemyAvatarId { get; set; }

        public FightResoult(bool won, List<FightTurn> turns, long playerHealth, long enemyHealth, long playerAvatarId, long enemyAvatarId)
        {
            this.Won = won;
            this.Turns = turns;
            this.PlayerHealth = playerHealth;
            this.EnemyHealth = enemyHealth;
            this.PlayerAvatarId = playerAvatarId;
            this.EnemyAvatarId = enemyAvatarId;
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