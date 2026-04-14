using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public class EnemyRenderer
    {
        private float _timer;
        private readonly float _timePerFrame;
        private readonly int _frameCount;

        public int CurrentFrame { get; private set; }

        public EnemyRenderer(int frameCount, float timePerFrame)
        {
            _frameCount = frameCount;
            _timePerFrame = timePerFrame;
            CurrentFrame = 0;
            _timer = 0f;
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (_timer >= _timePerFrame)
            {
                CurrentFrame = (CurrentFrame + 1) % _frameCount;
                _timer -= _timePerFrame;
            }
        }
    }

}
