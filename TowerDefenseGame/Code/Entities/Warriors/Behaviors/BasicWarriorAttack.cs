namespace TowerDefenseGame
{
    public class BasicWarriorAttack : BaseCombatBehavior, IWarriorAttack
    {
        private const float AttackRange = 15f;
        public BasicWarriorAttack() => AttackInterval = 3f;

        public void Attack(Warrior warrior, Enemy target, float deltaTime)
        {
            if (target == null || !target.IsAlive || !IsInRange(warrior.Position, target.Position, AttackRange)) return;

            warrior.ReduceAttackCooldown(deltaTime);
            
            if (warrior.AttackCooldown <= 0f)
            {
                target.TakeDamage(warrior.Damage);
                warrior.ResetAttackCooldown(AttackInterval);
                warrior.TriggerAttackAnimation();
            }

            warrior.SetState(Warrior.WarriorState.Attacking);
        }
    }

}
