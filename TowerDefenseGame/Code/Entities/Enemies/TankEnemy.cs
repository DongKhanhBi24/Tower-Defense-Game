using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public class TankEnemy : Enemy
    {
        private const float EnemyHealth = 300;
        private const float EnemySpeed = 40f;
        public TankEnemy(List<Vector2> path, EnemyAssets assets) 
        : base(path, EnemyHealth, EnemySpeed,
        new NormalMovement(), new EnemyAttack(), assets)
        {
            Reward = 60;
        }
        public override int DamageToWarrior => 20;

    }
}
