using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public class EliteWarriorMovement : IWarriorMovement
    {
        public void Move(Warrior warrior, Enemy target, float deltaTime)
        {
            if (target == null || !target.IsAlive) return;

            Vector2 direction = target.Position - warrior.Position;
            if (direction.Length() > 1f)
            {
                direction.Normalize();
                warrior.Position += direction * warrior.Speed * deltaTime; 
                warrior.SetState(Warrior.WarriorState.Walking);
            }
        }
    }
}
