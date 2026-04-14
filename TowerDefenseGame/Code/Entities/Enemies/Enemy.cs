using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TowerDefenseGame
{
    public abstract class Enemy
    {
        // =================== Dependencies ===================
        private readonly IEnemyAttack _attackBehavior;
        private readonly IEnemyMovement _movementBehavior;
        private readonly EnemyRenderer _animator;

        // =================== Core Properties ===================
        public Vector2 Position { get; set; }
        public List<Vector2> Path { get; }
        public int PathIndex { get; set; }

        public float Health { get; protected set; }
        public float MaxHealth { get; protected set; }
        public float Speed { get; protected set; }
        public int Reward { get; protected set; } = 20;

        public bool HasGivenReward { get; set; } = false;
        public bool IsAlive => Health > 0;
        public int CurrentFrame => _animator.CurrentFrame;

        // =================== Combat State ===================
        public Warrior TargetedBy { get; set; }
        public bool IsTargeted => TargetedBy != null;
        public bool IsEngagedInCombat { get; private set; }

        // =================== Rendering ===================
        protected Texture2D Sprite { get; set; }
        public int FrameWidth { get; }
        public int FrameHeight { get; }
        public Texture2D GetTexture() => Sprite;

        // =================== Damage Configuration ===================
        public virtual int DamageToWarrior => 5;

        // =================== Constructor ===================
        protected Enemy(List<Vector2> path, float health, float speed,
                        IEnemyMovement movementBehavior, IEnemyAttack attackBehavior,
                        EnemyAssets assets)
        {
            Path = path;
            Position = path[0];
            PathIndex = 1;

            Health = MaxHealth = health;
            Speed = speed;

            _movementBehavior = movementBehavior;
            _attackBehavior = attackBehavior;

            _animator = new EnemyRenderer(assets.FrameCount, assets.FrameTime);

            FrameWidth = assets.FrameWidth;
            FrameHeight = assets.FrameHeight;
            Sprite = assets.SpriteSheet;
        }

        // =================== Main Update ===================
        public void Update(GameTime gameTime, List<Warrior> warriors, Gate gate, Player player)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateCombatState();

            if (IsAlive)
            {
                // Move toward gate if not in combat
                if (!IsEngagedInCombat && Move(delta))
                {
                    gate.TakeDamage(1);
                    TakeDamage(999); // Instantly die at gate
                }

                if (IsAlive)
                    Attack(warriors, delta);
            }

            // Reward player if killed
            if (!IsAlive && !HasGivenReward)
            {
                player.AddCoins(Reward);
                HasGivenReward = true;
            }

            UpdateAnimation(gameTime);
        }

        // =================== Core Behaviors ===================
        private void UpdateCombatState()
        {
            IsEngagedInCombat = TargetedBy != null && TargetedBy.IsAlive &&
                                Vector2.Distance(Position, TargetedBy.Position) < 30f;
        }

        public virtual bool Move(float deltaTime)
        {
            return _movementBehavior.Move(this, deltaTime);
        }

        public virtual void Attack(List<Warrior> warriors, float deltaTime)
        {
            _attackBehavior?.Attack(this, warriors, deltaTime);
        }

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Health = 0;
                TargetedBy = null;
            }
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            _animator.Update(gameTime);
        }
    }
}
