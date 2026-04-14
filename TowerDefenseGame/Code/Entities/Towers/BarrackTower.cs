using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TowerDefenseGame
{
    public class BarrackTower : Tower
    {
        // =================== Fields ===================
        private readonly WarriorAssets _warriorAssets;
        private readonly List<Warrior> _warriors = new();

        private const float RespawnDelay = 5f;
        private float _respawnTimer = 0f;
        private bool _respawning = false;

        private int _maxWarriors = 3;

        private IWarriorAttack _currentAttackBehavior;
        private IWarriorMovement _currentMovementBehavior;

        // =================== Properties ===================
        public IEnumerable<Warrior> Warriors => _warriors;
        public override string TowerType => "Barrack";

        // =================== Constructor ===================
        public BarrackTower(Vector2 position, WarriorAssets warriorAssets)
            : base(position)
        {
            _warriorAssets = warriorAssets;
            SetLevel(Level);         // Initialize behavior and max warriors
            SpawnAllWarriors();      // Initial spawn
        }

        // =================== Update ===================
        public override void Update(float delta, List<Enemy> enemies, List<Projectile> projectiles)
        {
            UpdateWarriors(delta, enemies);
            HandleRespawn(delta);

            // Enemies respond to warriors
            foreach (var enemy in enemies)
                enemy.Attack(_warriors, delta);
        }

        // =================== Upgrade Logic ===================
        public override void Upgrade()
        {
            if (Level >= 4) return;

            Level++;
            SetLevel(Level);
            ForceReplaceWarriors();
        }

        // =================== Warrior Control ===================
        private void UpdateWarriors(float delta, List<Enemy> enemies)
        {
            foreach (var warrior in _warriors)
            {
                warrior.Update(delta, enemies);
                warrior.SeparateFromOtherWarriors(_warriors);
            }

            _warriors.RemoveAll(w => !w.IsAlive);
        }

        private void HandleRespawn(float delta)
        {
            if (_warriors.Count == 0 && !_respawning)
            {
                _respawning = true;
                _respawnTimer = RespawnDelay;
            }

            if (_respawning)
            {
                _respawnTimer -= delta;
                if (_respawnTimer <= 0f)
                {
                    SpawnAllWarriors();
                    _respawning = false;
                }
            }
        }

        private void SpawnAllWarriors()
        {
            for (int i = 0; i < _maxWarriors; i++)
            {
                var warrior = new Warrior(
                    Position,
                    _warriorAssets,
                    _currentMovementBehavior,
                    _currentAttackBehavior,
                    damage: GetWarriorDamage(),
                    speed: GetWarriorSpeed(),
                    health: GetWarriorHealth()
                );

                if (Level == 4)
                    warrior.SetEliteAppearance(_warriorAssets);

                _warriors.Add(warrior);
            }
        }

        private void ForceReplaceWarriors()
        {
            foreach (var warrior in _warriors)
                warrior.Kill();

            _warriors.Clear();
            _respawning = false;
            _respawnTimer = 0f;

            SpawnAllWarriors();
        }

        // =================== Level Configuration ===================
        private void SetLevel(int level)
        {
            _currentAttackBehavior = Level switch
            {
                1 => new BasicWarriorAttack(),
                2 => new FastWarriorAttack(),
                3 => new HeavyWarriorAttack(),
                4 => new EliteWarriorAttack(),
                _ => new BasicWarriorAttack()
            };

            _currentMovementBehavior = level == 4
                ? new EliteWarriorMovement()
                : new BasicWarriorMovement();

            _maxWarriors = level == 4 ? 4 : 3;
        }

        private int GetWarriorDamage() => Level switch
        {
            1 => 10,
            2 => 15,
            3 => 20,
            4 => 30,
            _ => 10
        };

        private float GetWarriorSpeed() => Level switch
        {
            1 => 90f,
            2 => 100f,
            3 => 110f,
            4 => 130f,
            _ => 90f
        };

        private int GetWarriorHealth() => Level switch
        {
            1 => 100,
            2 => 120,
            3 => 150,
            4 => 250,
            _ => 100
        };
    }
}
