using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefenseGame
{
    public class InputManager
    {
        private MouseState _previousMouse;
        private readonly Dictionary<string, Texture2D> _towerLevelTextures;
        private readonly Texture2D _upgradeIcon;

        public InputManager(RendererAssets assets)
        {
            _towerLevelTextures = assets.TowerTextures ?? throw new ArgumentNullException(nameof(assets.TowerTextures));
            _upgradeIcon = assets.UpgradeIcon ?? throw new ArgumentNullException(nameof(assets.UpgradeIcon));
        }

        public void HandleMouseInput(MouseState mouseState, Player player)
        {
            if (IsLeftClick(mouseState))
            {
                Vector2 clickPos = new(mouseState.X, mouseState.Y);
                var towerManager = GameManager.Instance.TowerManager;
                var rendererAssets = GameManager.Instance.RendererAssets;

                if (TryUpgradeSelectedTower(mouseState, towerManager, player)) return;
                if (towerManager.SelectTowerAt(clickPos)) { _previousMouse = mouseState; return; }

                if (towerManager.BuildSpot.HasValue &&
                    TryHandleBuildButtonClick(mouseState.Position, towerManager, player, rendererAssets)) return;

                if (towerManager.TrySelectBuildSpot(clickPos, GameManager.Instance.Map.ValidTowerSpots)) { _previousMouse = mouseState; return; }

                towerManager.ClearSelection();
                _previousMouse = mouseState;
            }

            _previousMouse = mouseState;
        }

        private bool IsLeftClick(MouseState current) =>
            current.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released;

        private bool TryUpgradeSelectedTower(MouseState mouseState, TowerManager towerManager, Player player)
        {
            var selected = towerManager.SelectedTower;
            if (selected == null) return false;

            Rectangle upgradeRect = GetUpgradeRect(selected.Position, selected.Level, selected.TowerType);
            if (towerManager.UpgradeTower(mouseState.Position, player, upgradeRect))
            {
                _previousMouse = mouseState;
                return true;
            }

            return false;
        }

        private bool TryHandleBuildButtonClick(Point click, TowerManager towerManager, Player player, RendererAssets assets)
        {
            var buildSpot = towerManager.BuildSpot ?? throw new Exception("Build spot should not be null here.");

            Rectangle magicRect = BuildMagicButtonRect(buildSpot, assets.HoverUI);
            Rectangle barrackRect = BuildBarrackButtonRect(buildSpot, assets.HoverUI);

            if (towerManager.HandleBuildButtonClick(click, player, magicRect, barrackRect))
            {
                _previousMouse = Mouse.GetState();
                return true;
            }

            return false;
        }

        private Rectangle GetUpgradeRect(Vector2 towerPos, int level, string towerType)
        {
            string key = $"{towerType}_{MathHelper.Clamp(level, 1, 4)}";

            if (!_towerLevelTextures.TryGetValue(key, out var texture))
                throw new Exception($"Texture not found for tower type '{towerType}' and level '{level}'");

            Vector2 position = towerPos - new Vector2(_upgradeIcon.Width / 2f, texture.Height / 2f + _upgradeIcon.Height);
            return new Rectangle((int)position.X, (int)position.Y, _upgradeIcon.Width, _upgradeIcon.Height);
        }

        private Rectangle BuildMagicButtonRect(Vector2 buildSpot, Texture2D hoverUI)
        {
            Vector2 topLeft = buildSpot - new Vector2(hoverUI.Width / 2f, hoverUI.Height / 2f);
            return new Rectangle((int)(topLeft.X + 10), (int)(topLeft.Y + hoverUI.Height - 55), 45, 45);
        }

        private Rectangle BuildBarrackButtonRect(Vector2 buildSpot, Texture2D hoverUI)
        {
            Vector2 topLeft = buildSpot - new Vector2(hoverUI.Width / 2f, hoverUI.Height / 2f);
            return new Rectangle((int)(topLeft.X + 10), (int)(topLeft.Y + 10), 45, 45);
        }
    }
}
