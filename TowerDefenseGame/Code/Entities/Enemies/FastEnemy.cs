using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public class FastEnemy : Enemy
    {
        private const float EnemyHealth = 75f;
        private const float EnemySpeed = 90f;
        
        public FastEnemy(List<Vector2> path, EnemyAssets assets)
        : base(path, EnemyHealth, EnemySpeed,
        new NormalMovement(), new EnemyAttack(), assets)
        {
            Reward = 30;
        }
        
        public override int DamageToWarrior => 10;
    }
}
