namespace TowerDefenseGame
{
    public class Player
    {
        // =================== Properties ===================
        public int Coins { get; private set; }

        // =================== Constructor ===================
        public Player(int startingCoins)
        {
            Coins = startingCoins;
        }

        // =================== Add coins to player ===================
        public void AddCoins(int amount)
        {
            Coins += amount;
            if (Coins < 0)
                Coins = 0;
        }
    }
}
