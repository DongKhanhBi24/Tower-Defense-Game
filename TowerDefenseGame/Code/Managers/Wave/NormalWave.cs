using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public class NormalWave : IWave
    {
        private int _count;
        private RendererAssets _assets;

        public NormalWave(int count, RendererAssets assets)
        {
            _count = count;
            _assets = assets;
        }

        public void SpawnEnemies(Queue<Enemy> queue, List<Vector2> path)
        {
            for (int i = 0; i < _count; i++)
                queue.Enqueue(EnemyFactory.CreateEnemy("Normal", path, _assets));
        }
    }
}
