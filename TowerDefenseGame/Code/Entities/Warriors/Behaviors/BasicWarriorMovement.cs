using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public class BasicWarriorMovement : IWarriorMovement
    {
        public void Move(Warrior warrior, Enemy target, float deltaTime)
        {
            if (target == null || !target.IsAlive) return;

            float distance = Vector2.Distance(warrior.Position, target.Position);
            if (distance > 15f)
            {
                warrior.Position = Vector2.Lerp(warrior.Position, target.Position, warrior.Speed * deltaTime / distance);
                warrior.SetState(Warrior.WarriorState.Walking);
            }
        }
    }
}
