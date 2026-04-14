using System.Collections.Generic;

namespace TowerDefenseGame
{
    public interface IEnemyAttack
    {
        void Attack(Enemy enemy, List<Warrior> warriors, float deltaTime); //Group-based
    }
}
