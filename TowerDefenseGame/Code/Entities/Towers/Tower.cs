using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TowerDefenseGame
{
    public abstract class Tower
    {
        public Vector2 Position { get; protected set; }
        public int Level { get; protected set; } = 1;

        protected Tower(Vector2 position)
        {
            Position = position;
        }

        public virtual void Upgrade()
        {
            Level = MathHelper.Clamp(Level + 1, 1, 4);
        }

        public abstract string TowerType { get; }  

        public abstract void Update(float deltaTime, List<Enemy> enemies, List<Projectile> projectiles);
    }
}
