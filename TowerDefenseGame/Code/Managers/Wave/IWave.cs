using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public interface IWave
    {
        public void SpawnEnemies(Queue<Enemy> queue, List<Vector2> path);
    }
}
