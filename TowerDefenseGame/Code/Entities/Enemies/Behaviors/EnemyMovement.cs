
namespace TowerDefenseGame
{
    public class NormalMovement : IEnemyMovement
    {
        public bool Move(Enemy enemy, float deltaTime)
        {
            if (enemy.PathIndex >= enemy.Path.Count)
                return true;

            var target = enemy.Path[enemy.PathIndex];
            var dir = target - enemy.Position;
            float distance = enemy.Speed * deltaTime;

            if (dir.Length() <= distance)
            {
                enemy.Position = target;
                enemy.PathIndex++;
                return enemy.PathIndex >= enemy.Path.Count;
            }

            dir.Normalize();
            enemy.Position += dir * distance;
            return false;
        }
    }
}
