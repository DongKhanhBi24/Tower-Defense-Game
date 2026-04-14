using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace TowerDefenseGame
{
    public class Warrior
    {
        // =================== States ===================
        public enum WarriorState { Walking, Attacking, Dead }

        // =================== Constants ===================
        private const float WalkFrameTime = 0.5f;
        private const float AttackDisplayTime = 0.2f;
        private const float DeathDisplayDuration = 6f;
        private const int TotalFrames = 4;
        private const float SeparationDistance = 30f;

        // =================== Components ===================
        private readonly IWarriorAttack _attackBehavior;
        private readonly IWarriorMovement _movementBehavior;
        private readonly WarriorRenderer _renderer;

        // =================== Internal State ===================
        private WarriorState _state;
        private Enemy _target;
        private Vector2 _position;

        private float _attackAnimTimer;
        private float _walkAnimTimer;
        private float _deathTimer;
        private int _currentFrame;

        // =================== Public Properties ===================
        public int Damage { get; private set; }
        public float Speed { get; private set; }
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public float AttackCooldown { get; private set; }
        public float DetectionRadius { get; private set; } = 200f;

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public bool IsAlive => _state != WarriorState.Dead || _deathTimer < DeathDisplayDuration;
        public WarriorState State => _state;

        // =================== Constructor ===================
        public Warrior(Vector2 position, WarriorAssets assets,
                       IWarriorMovement movement, IWarriorAttack attack,
                       int damage = 10, float speed = 90f, int health = 100)
        {
            _position = position;
            _movementBehavior = movement;
            _attackBehavior = attack;
            _renderer = new WarriorRenderer(this, assets, TotalFrames);

            Damage = damage;
            Speed = speed;
            Health = MaxHealth = health;
            AttackCooldown = 0f;
        }

        // =================== Update ===================
        public void Update(float delta, List<Enemy> enemies)
        {
            if (_state == WarriorState.Dead)
            {
                _deathTimer += delta;
                return;
            }

            UpdateTarget(enemies);
            _movementBehavior.Move(this, _target, delta);
            _attackBehavior.Attack(this, _target, delta);
            UpdateAnimation(delta);
        }

        // =================== Targeting ===================
        private void UpdateTarget(List<Enemy> enemies)
        {
            // Clear target if it's dead or untargeted
            if (_target != null && (!_target.IsAlive || _target.TargetedBy != this))
            {
                _target.TargetedBy = null;
                _target = null;
            }

            // Acquire new target if none
            if (_target == null)
            {
                var nearestEnemy = enemies
                    .Where(e => e.IsAlive && !e.IsTargeted &&
                                Vector2.Distance(_position, e.Position) <= DetectionRadius)
                    .OrderBy(e => Vector2.Distance(_position, e.Position))
                    .FirstOrDefault();

                if (nearestEnemy != null)
                {
                    _target = nearestEnemy;
                    _target.TargetedBy = this;
                }
            }
        }

        // =================== Animation ===================
        private void UpdateAnimation(float delta)
        {
            switch (_state)
            {
                case WarriorState.Walking:
                    _walkAnimTimer += delta;
                    if (_walkAnimTimer >= WalkFrameTime)
                    {
                        _walkAnimTimer = 0f;
                        _currentFrame = (_currentFrame + 1) % TotalFrames;
                    }
                    break;

                case WarriorState.Attacking:
                    _attackAnimTimer -= delta;
                    if (_attackAnimTimer <= 0f)
                        _state = WarriorState.Walking;
                    break;
            }
        }

        public void TriggerAttackAnimation()
        {
            _attackAnimTimer = AttackDisplayTime;
            _state = WarriorState.Attacking;
        }

        // =================== Combat ===================
        public void ReduceAttackCooldown(float deltaTime) => AttackCooldown -= deltaTime;
        public void ResetAttackCooldown(float value) => AttackCooldown = value;

        public void TakeDamage(int damage)
        {
            if (!IsAlive) return;

            Health -= damage;
            if (Health <= 0) Kill();
        }

        public void Kill()
        {
            // Release target
            if (_target != null)
            {
                _target.TargetedBy = null;
                _target = null;
            }

            // Set death state
            if (_state != WarriorState.Dead)
            {
                _state = WarriorState.Dead;
                _deathTimer = 0f;
            }
        }

        // =================== State & Separation ===================
        public void SetState(WarriorState state) => _state = state;

        // Prevents warriors from stacking on top of each other.
        public void SeparateFromOtherWarriors(List<Warrior> allWarriors)
        {
            foreach (var other in allWarriors)
            {
                if (other == this || !other.IsAlive) continue;

                float dist = Vector2.Distance(_position, other.Position);
                if (dist < SeparationDistance && dist > 0f)
                {
                    Vector2 pushDir = _position - other.Position;
                    pushDir.Normalize();
                    _position += pushDir * 2f; // push away slightly
                }
            }
        }

        // =================== Visuals ===================
        public void SetEliteAppearance(WarriorAssets assets) => _renderer.UseEliteTextures(assets);
        public Texture2D GetCurrentTexture() => _renderer.GetTexture();
        public Rectangle GetSourceRectangle() => _renderer.GetSourceRectangle(_currentFrame);
    }
}
