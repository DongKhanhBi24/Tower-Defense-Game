using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TowerDefenseGame
{
    public class RendererAssets
    {
        // Core
        public Texture2D Pixel { get; set; }
        public SpriteFont Font { get; set; }

        // UI & HUD
        public Texture2D UpgradeIcon { get; set; }
        public Texture2D HoverUI { get; set; }
        public Texture2D HUDCoins { get; set; }
        public Texture2D HUDGate { get; set; }
        public Texture2D HUDWave { get; set; }

        // Map & Spots
        public Texture2D TowerSpot { get; set; }
        public Texture2D MapBackground { get; set; }

        // Screens
        public Texture2D MainMenuTexture { get; set; }
        public Texture2D LoadingTexture { get; set; }
        public Texture2D PauseTexture { get; set; }
        public Texture2D VictoryTexture { get; set; }
        public Texture2D GameOverTexture { get; set; }
        public Texture2D InstructionTexture { get; set; }

        // Main
        public WarriorAssets WarriorAssets { get; set; }
        public Dictionary<string, Texture2D> TowerTextures { get; set; }
        public Dictionary<string, Texture2D> ProjectileTextures { get; set; }
        public Dictionary<string, EnemyAssets> EnemyAssets { get; set; }
    }
}
