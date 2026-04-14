using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseGame
{
    public class DisplayManager
    {
        private readonly GraphicsDeviceManager _graphics;

        public int PreferredWidth { get; private set; }
        public int PreferredHeight { get; private set; }

        public DisplayManager(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
        }

        public void SetResolution(int width, int height)
        {
            PreferredWidth = width;
            PreferredHeight = height;

            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.ApplyChanges();
        }
    }
}
