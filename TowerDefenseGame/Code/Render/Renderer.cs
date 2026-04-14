using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace TowerDefenseGame
{
    public class Renderer
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly RendererAssets _assets;

        private const int HealthBarWidth = 40;
        private const int HealthBarHeight = 4;

        public Renderer(SpriteBatch spriteBatch, RendererAssets assets)
        {
            _spriteBatch = spriteBatch;
            _assets = assets;
        }

        // ======================= Main Draw Methods =======================

        public void DrawMapBackground(GraphicsDevice graphicsDevice)
        {
            _spriteBatch.Draw(
                _assets.MapBackground,
                new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height),
                Color.White
            );
        }

        public void DrawValidTowerSpots(IEnumerable<Vector2> spots)
        {
            foreach (var spot in spots)
            {
                _spriteBatch.Draw(
                    _assets.TowerSpot,
                    spot,
                    null,
                    Color.White,
                    0f,
                    GetCenterOrigin(_assets.TowerSpot),
                    1.3f,
                    SpriteEffects.None,
                    0f
                );
            }
        }

        public void DrawTowers(IEnumerable<Tower> towers, Tower selectedTower)
        {
            foreach (var tower in towers)
            {
                string key = $"{tower.TowerType}_{MathHelper.Clamp(tower.Level, 1, 4)}";

                // Draw tower sprite
                if (_assets.TowerTextures.TryGetValue(key, out var texture))
                {
                    _spriteBatch.Draw(
                        texture,
                        tower.Position,
                        null,
                        Color.White,
                        0f,
                        GetCenterOrigin(texture),
                        1.3f,
                        SpriteEffects.None,
                        0f
                    );
                }

                // Draw upgrade icon if selected
                if (selectedTower == tower)
                {
                    var upgradeRect = GetUpgradeRect(tower);
                    _spriteBatch.Draw(_assets.UpgradeIcon, new Vector2(upgradeRect.X, upgradeRect.Y), Color.White);
                }
            }
        }

        public void DrawEnemies(IEnumerable<Enemy> enemies)
        {
            foreach (var enemy in enemies)
            {
                if (!enemy.IsAlive) continue;

                var texture = enemy.GetTexture();
                if (texture == null) continue;

                var sourceRect = new Rectangle(
                    enemy.CurrentFrame * enemy.FrameWidth,
                    0,
                    enemy.FrameWidth,
                    enemy.FrameHeight
                );

                _spriteBatch.Draw(
                    texture,
                    enemy.Position,
                    sourceRect,
                    Color.White,
                    0f,
                    GetCenterOrigin(sourceRect),
                    1.1f,
                    SpriteEffects.None,
                    0f
                );

                DrawHealthBar(enemy.Position, enemy.Health, enemy.MaxHealth, enemy.FrameHeight);
            }
        }

        public void DrawWarriors(IEnumerable<Warrior> warriors)
        {
            foreach (var warrior in warriors.Where(w => w.IsAlive))
            {
                var texture = warrior.GetCurrentTexture();
                var sourceRect = warrior.GetSourceRectangle();

                _spriteBatch.Draw(
                    texture,
                    warrior.Position,
                    sourceRect,
                    Color.White,
                    0f,
                    GetCenterOrigin(sourceRect),
                    1f,
                    SpriteEffects.None,
                    0f
                );

                if (warrior.State != Warrior.WarriorState.Dead)
                {
                    DrawHealthBar(warrior.Position, warrior.Health, warrior.MaxHealth, sourceRect.Height);
                }
            }
        }

        public void DrawProjectiles(IEnumerable<Projectile> projectiles)
        {
            foreach (var projectile in projectiles)
            {
                if (!_assets.ProjectileTextures.TryGetValue(projectile.ProjectileType, out var texture))
                    continue;

                var sourceRect = new Rectangle(
                    projectile.CurrentFrame * projectile.FrameWidth,
                    0,
                    projectile.FrameWidth,
                    texture.Height
                );

                _spriteBatch.Draw(
                    texture,
                    projectile.Position,
                    sourceRect,
                    Color.White,
                    0f,
                    GetCenterOrigin(sourceRect),
                    1f,
                    SpriteEffects.None,
                    0f
                );
            }
        }

        public void DrawHUDText(int coins, int gateHealth, int currentWave, int totalWaves)
        {
            // Coins
            _spriteBatch.Draw(_assets.HUDCoins, new Vector2(2, 2), Color.White);
            _spriteBatch.DrawString(_assets.Font, $"{coins}", new Vector2(66, 18), Color.White);

            // Gate Health
            _spriteBatch.Draw(_assets.HUDGate, new Vector2(152, 2), Color.White);
            _spriteBatch.DrawString(_assets.Font, $"{gateHealth}", new Vector2(216, 18), Color.White);

            // Wave Info
            _spriteBatch.Draw(_assets.HUDWave, new Vector2(3, 60), Color.White);
            _spriteBatch.DrawString(_assets.Font, $"Wave: {currentWave} / {totalWaves}", new Vector2(100, 76), Color.Yellow);
        }

        public void DrawHoverSpot(Vector2? hoveredSpot)
        {
            if (!hoveredSpot.HasValue) return;

            _spriteBatch.Draw(
                _assets.HoverUI,
                hoveredSpot.Value,
                null,
                Color.White,
                0f,
                GetCenterOrigin(_assets.HoverUI),
                1f,
                SpriteEffects.None,
                0f
            );
        }

        // ======================= Helper Methods =======================

        private void DrawHealthBar(Vector2 position, float currentHealth, float maxHealth, int spriteHeight)
        {
            float healthPct = MathHelper.Clamp(currentHealth / maxHealth, 0f, 1f);

            Vector2 barPosition = position - new Vector2(
                HealthBarWidth / 2f - 20,
                spriteHeight / 2f + 5
            );

            // Background (red)
            _spriteBatch.Draw(
                _assets.Pixel,
                new Rectangle((int)barPosition.X, (int)barPosition.Y, HealthBarWidth, HealthBarHeight),
                Color.DarkRed
            );

            // Foreground (green)
            _spriteBatch.Draw(
                _assets.Pixel,
                new Rectangle((int)barPosition.X, (int)barPosition.Y, (int)(HealthBarWidth * healthPct), HealthBarHeight),
                Color.Lime
            );
        }

        private Rectangle GetUpgradeRect(Tower tower)
        {
            string key = $"{tower.TowerType}_{MathHelper.Clamp(tower.Level, 1, 4)}";

            if (_assets.TowerTextures.TryGetValue(key, out var texture))
            {
                Vector2 position = tower.Position - new Vector2(
                    _assets.UpgradeIcon.Width / 2f,
                    texture.Height / 2f + _assets.UpgradeIcon.Height
                );

                return new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    _assets.UpgradeIcon.Width,
                    _assets.UpgradeIcon.Height
                );
            }

            return Rectangle.Empty;
        }

        private Vector2 GetCenterOrigin(Texture2D texture)
        {
            return new Vector2(texture.Width / 2f, texture.Height / 2f);
        }

        private Vector2 GetCenterOrigin(Rectangle rect)
        {
            return new Vector2(rect.Width / 2f, rect.Height / 2f);
        }
    }
}
