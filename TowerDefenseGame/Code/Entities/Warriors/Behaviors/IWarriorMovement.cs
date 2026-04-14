namespace TowerDefenseGame
{
    public interface IWarriorMovement
    {
        public void Move(Warrior warrior, Enemy target, float deltaTime);
    }
}
