using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public class Gate
    {
        // =================== Properties ===================
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public bool IsDestroyed => Health <= 0;

        // =================== Constructor ===================
        public Gate(int maxHealth)
        {
            MaxHealth = maxHealth;
            Health = maxHealth;
        }

        // =================== Gate take damage ===================

        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health < 0)
                Health = 0;

            if (IsDestroyed && GameManager.Instance.UIManager.CurrentState != UIState.GameOver)
                GameManager.Instance.UIManager.CurrentState = UIState.GameOver;
        }
    }
}
