using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public static class TowerFactory
    {
        private static TowerAssetBundle _assets;

        public static void Initialize(TowerAssetBundle assets)
        {
            _assets = assets;
        }

        public static Tower CreateTower(string type, Vector2 position)
        {
            return type switch
            {
                "Magic" => new MagicTower(position, new MagicTowerAssets
                {
                    MagicTextures = _assets.MagicTowerTextures
                }),
                "Barrack" => new BarrackTower(position, _assets.WarriorAssets),
                _ => throw new System.ArgumentException($"Tower type {type} is not supported"),
            };
        }
    }
}

