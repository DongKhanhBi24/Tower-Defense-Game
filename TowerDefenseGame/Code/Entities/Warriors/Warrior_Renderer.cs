using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseGame
{
   
    // Handles selecting the correct warrior texture and source rectangle for rendering.
    // Supports walking animation, attack, and death states.
    public class WarriorRenderer
    {
        // =================== Fields ===================
        private Texture2D _walkTexture;
        private Texture2D _attackTexture;
        private Texture2D _deathTexture;

        private readonly Warrior _warrior;
        private readonly int _totalFrames;

        // =================== Constructor ===================
        public WarriorRenderer(Warrior warrior, WarriorAssets assets, int totalFrames = 4)
        {
            _warrior = warrior;
            _totalFrames = totalFrames;

            // Default to normal textures
            _walkTexture = assets.WalkTexture;
            _attackTexture = assets.AttackTexture;
            _deathTexture = assets.DeathTexture;
        }

        // =================== Texture Management ===================
        // Switches the warrior to use elite textures (for level 4 barracks)
        public void UseEliteTextures(WarriorAssets assets)
        {
            _walkTexture = assets.EliteWalkTexture;
            _attackTexture = assets.EliteAttackTexture;
            _deathTexture = assets.EliteDeathTexture;
        }


        // Returns the correct texture based on the warrior's current state
        public Texture2D GetTexture() => _warrior.State switch
        {
            Warrior.WarriorState.Attacking => _attackTexture,
            Warrior.WarriorState.Dead      => _deathTexture,
            _                               => _walkTexture
        };


        // Returns the source rectangle for the current animation frame
        // Walking uses sprite-sheet frames, attack/death use single frames
        public Rectangle GetSourceRectangle(int currentFrame)
        {
            var texture = GetTexture();

            if (_warrior.State == Warrior.WarriorState.Walking)
            {
                int frameWidth = texture.Width / _totalFrames;
                return new Rectangle(currentFrame * frameWidth, 0, frameWidth, texture.Height);
            }

            // Attack and death states use a single frame
            return new Rectangle(0, 0, texture.Width, texture.Height);
        }
    }
}
