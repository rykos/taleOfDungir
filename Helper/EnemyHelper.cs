using taleOfDungir.Models;

namespace taleOfDungir.Helpers
{
    public static class EnemyHelper
    {
        public static Monster CreateEnemy(int level)
        {
            return new Monster(){
                Health = 10 * level,
                Level = level,
                Name = $"Monster lvl {level}",
                Damage = 5 * level
            };
        }
    }
}