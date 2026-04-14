using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public abstract class Projectile
    {
        protected float Speed = 200f;
        protected int _currentFrame = 0;
        protected bool _hit = false;

        public Vector2 Position { get; protected set; }
        public Enemy Target { get; }
        public int TowerLevel { get; }
        public bool IsActive => !_hit && Target.IsAlive;
        public int CurrentFrame => _currentFrame;


        public virtual int FrameWidth => 25;      
        public virtual int FrameCount => 1;       
        public abstract string ProjectileType { get; }


        protected Projectile(Vector2 position, Enemy target, int towerLevel)
        {
            Position = position;
            Target = target;
            TowerLevel = towerLevel;
        }

        public virtual void Update(float deltaTime)
        {
            if (_hit || !Target.IsAlive)
                return;

            Vector2 direction = Target.Position - Position;
            float travel = Speed * deltaTime;

            if (direction.Length() <= travel)
            {
                Position = Target.Position;
                Hit();
                return;
            }

            direction.Normalize();
            Position += direction * travel;
        }

        protected virtual void Hit(){
            _hit = true;
        }
    }
}
