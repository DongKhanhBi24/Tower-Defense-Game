using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public class AnimatedProjectile : Projectile
    {
        private float _timer;
        private const float TimePerFrame = 0.1f;

        public AnimatedProjectile(Vector2 position, Enemy target, int towerLevel)
            : base(position, target, towerLevel)
        {
            Position = position;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            _timer += deltaTime;
            if (_timer >= TimePerFrame)
            {
                _currentFrame = (_currentFrame + 1) % FrameCount;
                _timer = 0f;
            }
        }

        protected override void Hit()
        {
            base.Hit();
            Target.TakeDamage(25);
        }

        public override string ProjectileType => "AnimatedProjectile";
        public override int FrameCount => 2;
        public override int FrameWidth => 30;
    }
}
