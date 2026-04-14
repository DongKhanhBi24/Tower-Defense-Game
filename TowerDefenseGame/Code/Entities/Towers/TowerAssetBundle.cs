using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TowerDefenseGame
{
    public class TowerAssetBundle
    {
        public Dictionary<string, Texture2D> MagicTowerTextures { get; set; }
        public WarriorAssets WarriorAssets { get; set; }
    }
}
