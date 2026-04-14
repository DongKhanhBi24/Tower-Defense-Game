using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseGame
{
    public class GameManager
    {
        // =================== Singleton ===================
        private static GameManager _instance;
        public static GameManager Instance => _instance ??= new GameManager();

        // =================== Fields ===================
        private ContentManager _content;
        private GraphicsDevice _graphicsDevice;

        // =================== Game Components ===================
        public Player Player { get; private set; }
        public Gate Gate { get; private set; }
        public Map Map { get; private set; }

        public TowerManager TowerManager { get; private set; }
        public WaveManager WaveManager { get; private set; }
        public UIManager UIManager { get; private set; }
        public InputManager InputManager { get; private set; }
        public RendererAssets RendererAssets { get; private set; }

        // =================== Constructor ===================
        private GameManager() { }

        // =================== Initialization ===================
        public void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _content = content;
            _graphicsDevice = graphicsDevice;

            LoadAssets();
            InitializeFactories();
            InitializeManagers();

            ResetGame();
        }

        private void LoadAssets()
        {
            var loader = new ContentLoader(_content, _graphicsDevice);
            RendererAssets = loader.LoadRendererAssets();
        }

        private void InitializeFactories()
        {
            TowerFactory.Initialize(new TowerAssetBundle
            {
                MagicTowerTextures = RendererAssets.TowerTextures,
                WarriorAssets = RendererAssets.WarriorAssets
            });
        }

        private void InitializeManagers()
        {
            UIManager = new UIManager
            {
                CurrentState = UIState.MainMenu
            };
        }

        // =================== Update ===================
        public void Update(GameTime gameTime)
        {
            if (HandleGameOverOrVictory())
                return;

            UIManager.Update(gameTime);

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TowerManager.Update(delta, WaveManager.Enemies);
            WaveManager.Update(gameTime, this);
        }

        private bool HandleGameOverOrVictory()
        {
            if (Gate.IsDestroyed && UIManager.CurrentState != UIState.GameOver)
            {
                UIManager.CurrentState = UIState.GameOver;
                return true;
            }

            if (WaveManager.IsVictory)
            {
                UIManager.CurrentState = UIState.Victory;
                return true;
            }

            return false;
        }

        // =================== Reset Game ===================
        public void ResetGame()
        {
            Player = new Player(1000);
            Gate = new Gate(10);

            Map = new Map();
            Map.LoadPathFromFile("path.txt");

            TowerManager = new TowerManager();

            WaveManager = new WaveManager(Map.EnemyPath, RendererAssets);
            InputManager = new InputManager(RendererAssets);

            WaveManager.PrepareWave();
            UIManager.CurrentState = UIState.MainMenu;
        }
    }
}
