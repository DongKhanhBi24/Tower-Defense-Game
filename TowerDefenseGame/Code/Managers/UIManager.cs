using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefenseGame
{
    public enum UIState
    {
        MainMenu,
        Loading,
        HUD,
        Pause,
        Victory,
        GameOver,
        Instruction
    }

    public class UIManager
    {
        public Texture2D MainMenuTexture { get; set; }
        public Texture2D LoadingTexture { get; set; }
        public Texture2D PauseTexture { get; set; }
        public Texture2D VictoryTexture { get; set; }
        public Texture2D GameOverTexture { get; set; }
        public Texture2D InstructionTexture { get; set; }
        public UIState CurrentState { get; set; } = UIState.MainMenu;

        private MouseState _prevMouse;
        private KeyboardState _prevKeyboard;
        private Texture2D _fadeTexture;
        private Rectangle _startButtonRect, _moreButtonRect, _closeButtonRect, _restartButtonRect, _loadingBarFrameRect,
        _gameover_restartButtonRect, _gameover_menuButtonRect,
        _win_restartButtonRect, _win_menuButtonRect;
        private bool _isHoveringMore, _isHoveringStart, _isHoveringClose, _isHoveringRestart,
        _isHover_gameover_restartButtonRect, _isHover_gameover_menuButtonRect,
        _isHover_win_restartButtonRect, _isHover_win_menuButtonRect;
        private float _loadingProgress = 0f;

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            // Create a 1x1 white pixel use for fade overlay
            _fadeTexture = new Texture2D(graphicsDevice, 1, 1);
            _fadeTexture.SetData([Color.White]);

            _startButtonRect = new Rectangle(303, 280, 169, 50);
            _moreButtonRect = new Rectangle(213, 376, 130, 40);
            _closeButtonRect = new Rectangle(339, 390, 120, 35);
            _restartButtonRect = new Rectangle(555, 466, 133, 36);
            _loadingBarFrameRect = new Rectangle(238, 314, 316, 35);
            _gameover_restartButtonRect = new Rectangle(252, 306, 134, 38);
            _gameover_menuButtonRect = new Rectangle(419, 306, 134, 38);
            _win_restartButtonRect = new Rectangle(174, 299, 132, 38);
            _win_menuButtonRect = new Rectangle(336, 299, 132, 38);
        }
        
        public void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            var keyboard = Keyboard.GetState();

            switch (CurrentState)
            {
                case UIState.MainMenu:
                    _isHoveringStart = _startButtonRect.Contains(mouse.Position);
                    if (Clicked(mouse) && _isHoveringStart)
                        CurrentState = UIState.Loading;

                    _isHoveringMore = _moreButtonRect.Contains(mouse.Position);
                    if (Clicked(mouse) && _isHoveringMore)
                        CurrentState = UIState.Instruction;
                    break;

                case UIState.Loading:
                    _loadingProgress += (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;
                    if (_loadingProgress >= 1f)
                    {
                        CurrentState = UIState.HUD;
                        _loadingProgress = 0f;
                    }
                    break;
                
                case UIState.Instruction:
                    _isHoveringClose = _closeButtonRect.Contains(mouse.Position);
                    if (Clicked(mouse) && _isHoveringClose)
                        CurrentState = UIState.MainMenu;
                    break;

                case UIState.HUD:
                    if (PressedKey(keyboard, Keys.P))
                        CurrentState = UIState.Pause;
                    break;

                case UIState.Pause:
                    if (PressedKey(keyboard, Keys.P) || PressedKey(keyboard, Keys.Escape))//
                        CurrentState = UIState.HUD;
                    
                    _isHoveringRestart = _restartButtonRect.Contains(mouse.Position);
                    if (Clicked(mouse) && _isHoveringRestart)
                    {
                        GameManager.Instance.ResetGame();
                        CurrentState = UIState.HUD;
                    }
                    break;

                case UIState.Victory:
                    _isHover_win_restartButtonRect = _win_restartButtonRect.Contains(mouse.Position);
                    _isHover_win_menuButtonRect = _win_menuButtonRect.Contains(mouse.Position);
                    if (Clicked(mouse) && _isHover_win_restartButtonRect)
                    {
                        GameManager.Instance.ResetGame();
                        CurrentState = UIState.HUD;
                    }
                    else if (Clicked(mouse) && _isHover_win_menuButtonRect)
                    {
                        GameManager.Instance.ResetGame();
                        CurrentState = UIState.MainMenu;
                    }
                    break;

                case UIState.GameOver:
                    _isHover_gameover_restartButtonRect = _gameover_restartButtonRect.Contains(mouse.Position);
                    _isHover_gameover_menuButtonRect = _gameover_menuButtonRect.Contains(mouse.Position);
                    if (Clicked(mouse) && _isHover_gameover_restartButtonRect)
                    {
                        GameManager.Instance.ResetGame();
                        CurrentState = UIState.HUD;
                    }
                    else if (Clicked(mouse) && _isHover_gameover_menuButtonRect)
                    {
                        GameManager.Instance.ResetGame();
                        CurrentState = UIState.MainMenu;
                    }
                    break;
            }

            _prevMouse = mouse;
            _prevKeyboard = keyboard;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            var viewport = graphics.Viewport;
            Rectangle destRect = new(0, 0, 805, 473);
            switch (CurrentState)
            {
                case UIState.MainMenu:
                    spriteBatch.Draw(MainMenuTexture, destRect, Color.White);

                    // Draw hovering "StartDefense" and "More" button
                    Color color_s = _isHoveringStart ? Color.LightYellow * 0.15f : Color.Transparent;
                    Color color_m = _isHoveringMore ? Color.LightYellow * 0.2f : Color.Transparent;
                    
                    spriteBatch.Draw(_fadeTexture, _startButtonRect, color_s);
                    spriteBatch.Draw(_fadeTexture, _moreButtonRect, color_m);
                    break;

                case UIState.Instruction:
                    spriteBatch.Draw(InstructionTexture, destRect, Color.White);

                    // Draw hovering "Close" button
                    Color color_c = _isHoveringClose ? Color.LightYellow * 0.15f : Color.Transparent;
                    spriteBatch.Draw(_fadeTexture, _closeButtonRect, color_c);
                    break;

                case UIState.Loading:
                    spriteBatch.Draw(LoadingTexture, destRect, Color.White);

                    // Draw progress fill bar
                    int fillWidth = (int)(_loadingBarFrameRect.Width * MathHelper.Clamp(_loadingProgress, 0f, 1f));
                    Rectangle fillRect = new(
                        _loadingBarFrameRect.X,  
                        _loadingBarFrameRect.Y,
                        fillWidth,              
                        _loadingBarFrameRect.Height - 12);

                    spriteBatch.Draw(_fadeTexture, fillRect, Color.Yellow);
                    break;

                case UIState.Pause:
                    DrawHUDOverlay(spriteBatch, graphics);
                    Vector2 center = new(viewport.Width / 2f, viewport.Height / 2f);
                    spriteBatch.Draw(PauseTexture, center, null, Color.White, 0f,
                        new Vector2(PauseTexture.Width / 2f, PauseTexture.Height / 2f),
                        1f, SpriteEffects.None, 0f);
                    
                    // Draw hovering "Restart" button
                    Color color_r = _isHoveringRestart ? Color.LightYellow * 0.2f : Color.Transparent;
                    spriteBatch.Draw(_fadeTexture, _restartButtonRect, color_r);
                    break;

                case UIState.Victory:
                    DrawHUDOverlay(spriteBatch, graphics);
                    Vector2 centerVic = new(viewport.Width / 2f, viewport.Height / 2f);
                    spriteBatch.Draw(VictoryTexture, centerVic, null, Color.White, 0f,
                        new Vector2(VictoryTexture.Width / 2f, VictoryTexture.Height / 2f),
                        1f, SpriteEffects.None, 0f);

                    // Draw hovering "Restart" and "Menu" button
                    Color color_w_r = _isHover_win_restartButtonRect ? Color.LightYellow * 0.2f : Color.Transparent;
                    Color color_w_m = _isHover_win_menuButtonRect ? Color.LightYellow * 0.2f : Color.Transparent;
                    spriteBatch.Draw(_fadeTexture, _win_restartButtonRect, color_w_r);
                    spriteBatch.Draw(_fadeTexture, _win_menuButtonRect, color_w_m);
                    break;

                case UIState.GameOver:
                    DrawHUDOverlay(spriteBatch, graphics);
                    Vector2 centerGO = new(viewport.Width / 2f, viewport.Height / 2f);
                    spriteBatch.Draw(GameOverTexture, centerGO, null, Color.White, 0f,
                        new Vector2(GameOverTexture.Width / 2f, GameOverTexture.Height / 2f),
                        1f, SpriteEffects.None, 0f);
                    
                    // Draw hovering "Restart" and "Menu" button
                    Color color_go_r = _isHover_gameover_restartButtonRect ? Color.LightYellow * 0.2f : Color.Transparent;
                    Color color_go_m = _isHover_gameover_menuButtonRect ? Color.LightYellow * 0.2f : Color.Transparent;
                    spriteBatch.Draw(_fadeTexture, _gameover_restartButtonRect, color_go_r);
                    spriteBatch.Draw(_fadeTexture, _gameover_menuButtonRect, color_go_m);
                    break;
            }
        }
        private void DrawHUDOverlay(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            Rectangle overlay = new(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);
            spriteBatch.Draw(_fadeTexture, overlay, Color.Black * 0.7f);  // 70% fade
        }

        private bool Clicked(MouseState mouse)
        {
            return mouse.LeftButton == ButtonState.Pressed && _prevMouse.LeftButton == ButtonState.Released;
        }

        private bool PressedKey(KeyboardState keyboard, Keys key)
        {
            return keyboard.IsKeyDown(key) && _prevKeyboard.IsKeyUp(key);
        }
    }
}
