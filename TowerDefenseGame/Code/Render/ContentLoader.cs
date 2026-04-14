using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TowerDefenseGame
{
    public class ContentLoader
    {
        private readonly ContentManager _content;
        private readonly GraphicsDevice _graphicsDevice;

        public ContentLoader(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _content = content;
            _graphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// Loads all assets required by the renderer.
        /// </summary>
        public RendererAssets LoadRendererAssets()
        {
            return new RendererAssets
            {
                // Core Assets
                Pixel = CreatePixelTexture(),
                Font = _content.Load<SpriteFont>("DefaultFont"),

                // UI & HUD
                UpgradeIcon = _content.Load<Texture2D>("Upgrade"),
                HoverUI = _content.Load<Texture2D>("Hover"),
                HUDCoins = _content.Load<Texture2D>("HUD_Coins"),
                HUDGate = _content.Load<Texture2D>("HUD_GateHealth"),
                HUDWave = _content.Load<Texture2D>("HUD_Wave"),

                // Map & Spots
                TowerSpot = _content.Load<Texture2D>("spot"),
                MapBackground = _content.Load<Texture2D>("Maps/Mapdefault"),

                // Screens
                MainMenuTexture = _content.Load<Texture2D>("UI_MainMenu"),
                LoadingTexture = _content.Load<Texture2D>("UI_Loading"),
                PauseTexture = _content.Load<Texture2D>("UI_Paused"),
                VictoryTexture = _content.Load<Texture2D>("UI_Win"),
                GameOverTexture = _content.Load<Texture2D>("UI_Lose"),
                InstructionTexture = _content.Load<Texture2D>("UI_Instruction"),

                // Assets
                TowerTextures = LoadTowerTextures(),
                ProjectileTextures = LoadProjectileTextures(),
                EnemyAssets = LoadEnemyAssets(),
                WarriorAssets = LoadWarriorAssets(),
            };
        }

        // Asset Loaders

        private Texture2D CreatePixelTexture()
        {
            var pixel = new Texture2D(_graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            return pixel;
        }

        private Dictionary<string, Texture2D> LoadTowerTextures()
        {
            var towerTextures = new Dictionary<string, Texture2D>();

            for (int level = 1; level <= 4; level++)
            {
                towerTextures[$"Magic_{level}"] = _content.Load<Texture2D>($"MagicalTowerlv{level}");
                towerTextures[$"Barrack_{level}"] = _content.Load<Texture2D>($"BarrackTowerlv{level}");
            }

            return towerTextures;
        }

        private Dictionary<string, Texture2D> LoadProjectileTextures()
        {
            return new Dictionary<string, Texture2D>
            {
                { "BasicProjectile", _content.Load<Texture2D>("projectile_MagicalTower") },
                { "AnimatedProjectile", _content.Load<Texture2D>("projectile_MagicalTowerlv4") }
            };
        }

        private Dictionary<string, EnemyAssets> LoadEnemyAssets()
        {
            return new Dictionary<string, EnemyAssets>
            {
                {
                    "Normal", new EnemyAssets
                    {
                        SpriteSheet = _content.Load<Texture2D>("Enemy_Normal"),
                        FrameWidth = 74,
                        FrameHeight = 50,
                        FrameCount = 2,
                        FrameTime = 0.25f
                    }
                },
                {
                    "Fast", new EnemyAssets
                    {
                        SpriteSheet = _content.Load<Texture2D>("Enemy_Fast"),
                        FrameWidth = 111,
                        FrameHeight = 72,
                        FrameCount = 2,
                        FrameTime = 0.25f
                    }
                },
                {
                    "Tank", new EnemyAssets
                    {
                        SpriteSheet = _content.Load<Texture2D>("Enemy_Tank2"),
                        FrameWidth = 111,
                        FrameHeight = 97,
                        FrameCount = 2,
                        FrameTime = 0.25f
                    }
                }
            };
        }

        public WarriorAssets LoadWarriorAssets()
        {
            return new WarriorAssets
            {
                // Normal Warrior
                WalkTexture = _content.Load<Texture2D>("Warrior1"),
                AttackTexture = _content.Load<Texture2D>("Warrior_Attack"),
                DeathTexture = _content.Load<Texture2D>("Warrior_Death"),

                // Elite Warrior
                EliteWalkTexture = _content.Load<Texture2D>("WarriorElite"),
                EliteAttackTexture = _content.Load<Texture2D>("WarriorElite_Attack"),
                EliteDeathTexture = _content.Load<Texture2D>("WarriorElite_Death")
            };
        }
    }
}
