using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public class WaveManager
    {
        // =================== Fields ===================
        private readonly List<Vector2> _path;
        private readonly RendererAssets _rendererAssets;

        private readonly Dictionary<int, IWave> _waveMap = new();
        private readonly Queue<Enemy> _spawnQueue = new();

        private float _spawnCooldown = 0f;
        private int _currentWave = 1;

        // =================== Properties ===================
        public List<Enemy> Enemies { get; } = new();
        public int CurrentWave => _currentWave;
        public int TotalWaves => _waveMap.Count;
        public bool IsVictory =>
            _currentWave > TotalWaves &&
            _spawnQueue.Count == 0 &&
            Enemies.All(e => !e.IsAlive);

        // =================== Constructor ===================
        public WaveManager(List<Vector2> path, RendererAssets assets)
        {
            _path = path;
            _rendererAssets = assets;
            InitializeWaves();
        }

        // =================== Initialization ===================
        private void InitializeWaves()
        {
            _waveMap[1] = new NormalWave(5, _rendererAssets);
            _waveMap[2] = new FastWave(5, _rendererAssets);
            _waveMap[3] = new TankWave(3, _rendererAssets);
            _waveMap[4] = new MixedWave(5, 5, 5, _rendererAssets);
        }

        // =================== Wave Management ===================
        public void PrepareWave()
        {
            _spawnQueue.Clear();

            if (_waveMap.TryGetValue(_currentWave, out var wave))
                wave.SpawnEnemies(_spawnQueue, _path);
            else
            {
                // Fallback: endless mixed waves after last wave
                var fallbackWave = new MixedWave(5, 3, 3, _rendererAssets);
                fallbackWave.SpawnEnemies(_spawnQueue, _path);
            }
        }

        // =================== Update ===================
        public void Update(GameTime gameTime, GameManager gameManager)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _spawnCooldown -= delta;

            SpawnEnemiesIfReady();

            // Update existing enemies
            foreach (var enemy in Enemies)
            {
                enemy.Update(
                    gameTime,
                    gameManager.TowerManager.Warriors.ToList(),
                    gameManager.Gate,
                    gameManager.Player
                );
            }

            CheckWaveCompletion();
        }

        // =================== Helpers ===================
        private void SpawnEnemiesIfReady()
        {
            if (_spawnQueue.Count > 0 && _spawnCooldown <= 0f)
            {
                var enemy = _spawnQueue.Dequeue();
                Enemies.Add(enemy);
                _spawnCooldown = 2f;
            }
        }

        private void CheckWaveCompletion()
        {
            if (_spawnQueue.Count == 0 && Enemies.All(e => !e.IsAlive))
            {
                _currentWave++;

                if (_currentWave <= TotalWaves)
                    PrepareWave();
            }
        }
    }
}
