using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public static class EnemyFactory
    {
        public static Enemy CreateEnemy(string enemyType, List<Vector2> path, RendererAssets rendererAssets)
        {
            if (!rendererAssets.EnemyAssets.ContainsKey(enemyType))
                throw new ArgumentException($"Enemy type '{enemyType}' not found in assets.");

            var enemyAssets = rendererAssets.EnemyAssets[enemyType];

            return enemyType switch
            {
                "Normal" => new NormalEnemy(path, enemyAssets),
                "Fast"   => new FastEnemy(path, enemyAssets),
                "Tank"   => new TankEnemy(path, enemyAssets),
                _ => throw new ArgumentException($"Enemy type '{enemyType}' is not supported.")
            };
        }
    }
}
