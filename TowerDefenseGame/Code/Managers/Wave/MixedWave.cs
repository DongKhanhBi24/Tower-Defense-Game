using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public class MixedWave : IWave
    {
        private int _count, _count2, _count3;
        private RendererAssets _assets;
        public MixedWave(int count, int count2, int count3, RendererAssets assets)
        {
            _count = count;
            _count2 = count2;
            _count3 = count3;
            _assets = assets;
        }

        public void SpawnEnemies(Queue<Enemy> queue, List<Vector2> path)
        {
            for (int i = 0; i < _count; i++)
                queue.Enqueue(EnemyFactory.CreateEnemy("Normal", path, _assets));
            for (int i = 0; i < _count2; i++)
                queue.Enqueue(EnemyFactory.CreateEnemy("Fast", path, _assets));
            for (int i = 0; i < _count3; i++)
                queue.Enqueue(EnemyFactory.CreateEnemy("Tank", path, _assets));
        }
    }
}
