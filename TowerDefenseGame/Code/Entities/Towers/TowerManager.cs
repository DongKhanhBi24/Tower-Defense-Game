using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TowerDefenseGame
{
    public class TowerManager
    {
        // =================== Constants ===================
        private const int UpgradeCost = 125;
        private const int MagicTowerCost = 100;
        private const int BarrackTowerCost = 75;
        private const float TowerSelectRadius = 30f;
        private const float SpotOccupiedRadius = 10f;

        // =================== Fields ===================
        private readonly List<Tower> _towers = new();
        private readonly List<Projectile> _projectiles = new();

        // =================== Properties ===================
        public Vector2? BuildSpot { get; private set; }
        public Tower SelectedTower { get; private set; }
        public IEnumerable<Tower> Towers => _towers;
        public IEnumerable<Projectile> Projectiles => _projectiles;
        public IEnumerable<Warrior> Warriors =>
            _towers.OfType<BarrackTower>().SelectMany(b => b.Warriors);

        // =================== Constructor ===================
        public TowerManager(){}

        // =================== Update ===================
        public void Update(float delta, List<Enemy> enemies)
        {
            foreach (var tower in _towers)
                tower.Update(delta, enemies, _projectiles);

            foreach (var projectile in _projectiles)
                projectile.Update(delta);

            _projectiles.RemoveAll(p => !p.IsActive);
        }

        // =================== Selection & Upgrade ===================
        public bool SelectTowerAt(Vector2 clickPos)
        {
            foreach (var tower in _towers)
            {
                if (Vector2.Distance(tower.Position, clickPos) < TowerSelectRadius)
                {
                    SelectedTower = tower;
                    BuildSpot = null;
                    return true;
                }
            }
            return false;
        }

        public bool UpgradeTower(Point mousePos, Player player, Rectangle upgradeRect)
        {
            if (SelectedTower == null || SelectedTower.Level >= 4)
                return false;

            if (upgradeRect.Contains(mousePos) && player.Coins >= UpgradeCost)
            {
                SelectedTower.Upgrade();
                player.AddCoins(-UpgradeCost);
                SelectedTower = null;
                return true;
            }

            return false;
        }

        public void ClearSelection()
        {
            SelectedTower = null;
            BuildSpot = null;
        }

        // =================== Build Spot Selection ===================
        public bool TrySelectBuildSpot(Vector2 clickPos, IEnumerable<Vector2> validTowerSpots)
        {
            foreach (var spot in validTowerSpots)
            {
                bool isSpotAvailable = !_towers.Any(t => Vector2.Distance(t.Position, spot) < SpotOccupiedRadius);

                if (Vector2.Distance(spot, clickPos) < TowerSelectRadius && isSpotAvailable)
                {
                    BuildSpot = spot;
                    SelectedTower = null;
                    return true;
                }
            }
            return false;
        }

        // =================== Building Towers ===================
        public bool HandleBuildButtonClick(Point mousePos, Player player, Rectangle magicRect, Rectangle barrackRect)
        {
            if (!BuildSpot.HasValue)
                return false;

            // Try to build Magic Tower
            if (TryBuildTower(mousePos, magicRect, MagicTowerCost, player, "Magic"))
                return true;

            // Try to build Barrack Tower
            if (TryBuildTower(mousePos, barrackRect, BarrackTowerCost, player, "Barrack"))
                return true;

            // Clicked outside both buttons → cancel build mode
            if (!magicRect.Contains(mousePos) && !barrackRect.Contains(mousePos))
                BuildSpot = null;

            return false;
        }

        private bool TryBuildTower(Point mousePos, Rectangle buttonArea, int cost, Player player, string towerType)
        {
            if (!buttonArea.Contains(mousePos) || player.Coins < cost)
                return false;

            // Create tower using the TowerFactory with towerType
            Tower newTower = TowerFactory.CreateTower(towerType, BuildSpot.Value);

            _towers.Add(newTower);
            player.AddCoins(-cost);
            BuildSpot = null;
            return true;
        }
    }
}
