using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public abstract class BaseCombatBehavior
    {
        protected float Cooldown;
        protected float AttackInterval;

        protected bool IsInRange(Vector2 a, Vector2 b, float range) =>
            Vector2.Distance(a, b) <= range;

        protected bool CooldownReady(float deltaTime)
        {
            Cooldown -= deltaTime;
            return Cooldown <= 0f;
        }

        protected void ResetCooldown()
        {
            Cooldown = AttackInterval;
        }
    }
}
