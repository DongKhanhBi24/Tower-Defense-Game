using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public class BasicProjectile : Projectile
    {
        public BasicProjectile(Vector2 position, Enemy target, int towerLevel)
            : base(position, target, towerLevel)
        {
            Position = position;
        }

        protected override void Hit()
        {
            base.Hit();
            int damage = 12 + (TowerLevel - 1) * 4;
            Target.TakeDamage(damage);
        }

        public override string ProjectileType => "BasicProjectile";
    }
}
