using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefenseGame
{
    /// <summary>
    /// Main MonoGame entry point and game loop for TowerDefenseGame
    /// Handles initialization, content loading, updating, and drawing
    /// </summary>
    public class Game1 : Game
    {
        // =================== Fields ===================
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private DisplayManager _displayManager;
        private Renderer _renderer;
        private InputManager _inputManager;
        

        // =================== Constructor ===================
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _displayManager = new DisplayManager(_graphics);
        }

        // =================== MonoGame Lifecycle ===================

      
        // Initializes all core managers and UI.
        protected override void Initialize()
        {
            // Initialize global GameManager and its subsystems
            var gameManager = GameManager.Instance;
            gameManager.Initialize(Content, GraphicsDevice);
            gameManager.UIManager.Initialize(GraphicsDevice);

            // Initialize input system
            _inputManager = new InputManager(gameManager.RendererAssets);

            base.Initialize();
        }

       
        // Loads all textures, sprites, and renderer assets.
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var loader = new ContentLoader(Content, GraphicsDevice);
            var assets = loader.LoadRendererAssets();
            _renderer = new Renderer(_spriteBatch, assets);

            // Assign UI textures
            var ui = GameManager.Instance.UIManager;
            ui.MainMenuTexture = assets.MainMenuTexture;
            ui.LoadingTexture = assets.LoadingTexture;
            ui.PauseTexture = assets.PauseTexture;
            ui.VictoryTexture = assets.VictoryTexture;
            ui.GameOverTexture = assets.GameOverTexture;
            ui.InstructionTexture = assets.InstructionTexture;
        }

       
        // Main update loop. Handles input, game logic, and UI updates per frame

        protected override void Update(GameTime gameTime)
        {
            var gameManager = GameManager.Instance;
            gameManager.UIManager.Update(gameTime);

            switch (gameManager.UIManager.CurrentState)
            {
                case UIState.HUD:
                    _displayManager.SetResolution(1243, 860);
                    _inputManager.HandleMouseInput(Mouse.GetState(), gameManager.Player);
                    gameManager.Update(gameTime);
                    break;

                case UIState.Pause:
                    // Game is paused; UI handled by Draw()
                    break;

                default:
                    _displayManager.SetResolution(805, 473);
                    break;
            }

            base.Update(gameTime);
        }

  
        // Main draw loop. Clears screen and draws background, UI, and gameplay.
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            var uiState = GameManager.Instance.UIManager.CurrentState;
            switch (uiState)
            {
                case UIState.HUD:
                    DrawGamePlay();
                    break;

                case UIState.Pause:
                    DrawGamePlay(); // Draw game underneath pause menu
                    GameManager.Instance.UIManager.Draw(_spriteBatch, GraphicsDevice);
                    break;

                default:
                    GameManager.Instance.UIManager.Draw(_spriteBatch, GraphicsDevice);
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        // =================== Drawing Helpers ===================
        // Draws the entire gameplay layer: map, towers, enemies, projectiles, HUD

        private void DrawGamePlay()
        {
            var gameManager = GameManager.Instance;

            _renderer.DrawMapBackground(GraphicsDevice);
            _renderer.DrawValidTowerSpots(gameManager.Map.ValidTowerSpots);

            _renderer.DrawEnemies(gameManager.WaveManager.Enemies);
            _renderer.DrawWarriors(gameManager.TowerManager.Warriors);
            _renderer.DrawTowers(gameManager.TowerManager.Towers, gameManager.TowerManager.SelectedTower);
            _renderer.DrawProjectiles(gameManager.TowerManager.Projectiles);

            _renderer.DrawHUDText(
                gameManager.Player.Coins,
                gameManager.Gate.Health,
                gameManager.WaveManager.CurrentWave,
                gameManager.WaveManager.TotalWaves
            );

            _renderer.DrawHoverSpot(gameManager.TowerManager.BuildSpot);
        }
    }
}
