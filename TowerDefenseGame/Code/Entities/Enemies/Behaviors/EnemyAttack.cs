using System.Collections.Generic;

namespace TowerDefenseGame
{
    public class EnemyAttack : BaseCombatBehavior, IEnemyAttack
    {
        private readonly float _attackRange = 30f;
        public EnemyAttack()
        {
            Cooldown = 0f;
            AttackInterval = 2f;
        }

        public void Attack(Enemy enemy, List<Warrior> warriors, float deltaTime)
        {
            if (enemy == null || enemy.TargetedBy == null || !enemy.IsAlive) return;

            Warrior warrior = enemy.TargetedBy;
            if (!warrior.IsAlive) return;

            if (!IsInRange(enemy.Position, warrior.Position, _attackRange)) return;

            if (CooldownReady(deltaTime))
            {
                warrior.TakeDamage(enemy.DamageToWarrior);
                ResetCooldown();
            }
        }
    }
}
