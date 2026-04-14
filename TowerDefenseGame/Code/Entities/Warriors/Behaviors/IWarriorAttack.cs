namespace TowerDefenseGame
{
    public interface IWarriorAttack
    {
        public void Attack(Warrior warrior, Enemy target, float deltaTime); //1v1 targeting
    }
}
