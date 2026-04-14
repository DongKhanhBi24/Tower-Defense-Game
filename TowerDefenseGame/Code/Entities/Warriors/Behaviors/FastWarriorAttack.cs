using Microsoft.Xna.Framework;
namespace TowerDefenseGame
{
    public class FastWarriorAttack : BaseCombatBehavior, IWarriorAttack
    {
        private const float AttackRange = 15f;

        public FastWarriorAttack() => AttackInterval = 3f;

        public void Attack(Warrior warrior, Enemy target, float deltaTime)
        {
            if (target == null || !target.IsAlive) return;

            float distance = Vector2.Distance(warrior.Position, target.Position);
            if (distance <= AttackRange)
            {
                warrior.ReduceAttackCooldown(deltaTime);

                if (warrior.AttackCooldown <= 0f)
                {
                    target.TakeDamage(warrior.Damage);
                    warrior.ResetAttackCooldown(AttackRange);
                    warrior.TriggerAttackAnimation();
                }

                warrior.SetState(Warrior.WarriorState.Attacking);
            }
        }
    }
}