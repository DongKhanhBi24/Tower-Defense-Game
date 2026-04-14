using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public class NormalEnemy : Enemy
    {
        private const float EnemyHealth = 70;
        private const float EnemySpeed = 60f;
        public NormalEnemy(List<Vector2> path, EnemyAssets assets)
            : base(path, EnemyHealth, EnemySpeed,
            new NormalMovement(), new EnemyAttack(), assets)
        {
            Reward = 20;
        }
    }
}

