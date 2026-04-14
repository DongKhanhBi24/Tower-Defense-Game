using System;
using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public static class ProjectileFactory
    {
        public static Projectile CreateProjectile(string type, Vector2 position, Enemy target, int level)
        {
            return type switch
            {
                "BasicProjectile" => new BasicProjectile(position, target, level),
                "AnimatedProjectile" => new AnimatedProjectile(position, target, level),
                _ => throw new ArgumentException($"Unknown projectile type: {type}")
            };
        }
    }

}
