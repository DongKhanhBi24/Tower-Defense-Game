using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TowerDefenseGame
{
    public class MagicTower : Tower
    {
        // =================== Fields ===================
        private float _cooldown;
        private Func<Vector2, Enemy, int, Projectile> _projectileFactory;

        // =================== Constructor ===================
        public MagicTower(Vector2 position, MagicTowerAssets assets)
            : base(position)
        {
            SetLevel(Level); // Initialize projectile behavior and cooldown for current level
        }

        // =================== Update ===================
        public override void Update(float deltaTime, List<Enemy> enemies, List<Projectile> projectiles)
        {
            _cooldown -= deltaTime;
            if (_cooldown > 0f) return;

            // Select first valid enemy in range
            var target = enemies.FirstOrDefault(e =>
                e.IsAlive &&
                Vector2.Distance(Position, e.Position) < 250f);

            if (target == null) return;

            // Fire projectile and reset cooldown
            projectiles.Add(_projectileFactory(Position, target, Level));
            _cooldown = GetCooldownForLevel(Level);
        }

        // =================== Upgrade Logic ===================
        public override void Upgrade()
        {
            if (Level >= 4) return;

            Level++;
            SetLevel(Level); // Refresh projectile behavior and cooldown
        }

        // =================== Level Configuration ===================
        private void SetLevel(int level)
        {
            _cooldown = GetCooldownForLevel(level);

            // Select projectile creation logic based on tower level
            _projectileFactory = level switch
            {
                4 => (pos, target, lvl) => 
                    ProjectileFactory.CreateProjectile("AnimatedProjectile", pos, target, lvl),
                _ => (pos, target, lvl) => 
                    ProjectileFactory.CreateProjectile("BasicProjectile", pos, target, lvl)
            };
        }

        private float GetCooldownForLevel(int level) => level switch
        {
            1 => 1.2f,
            2 => 1.0f,
            3 => 0.8f,
            4 => 0.6f,
            _ => 1.0f
        };

        // =================== Type ===================
        public override string TowerType => "Magic";
    }
}
