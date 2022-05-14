using System;
using System.Collections.Generic;
using System.Text;
using ELEPHANTSRPG.StateManagement;
using ELEPHANTSRPG.Objects;
using ELEPHANTSRPG.Screens.TitleScreenObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ELEPHANTSRPG.Screens
{
    public class InstructionsScreen: GameScreen
    {
        private InputAction _startGame;
        private SpriteFont _bangers;
        private ContentManager _content;
        private Game _game;
        public InstructionsScreen(Game game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            _game = game;
            _startGame = new InputAction(
                new[] { Buttons.Start },
                new[] { Keys.Enter, Keys.Space },
                true);
        }
        public override void Activate()
        {
            if (_content == null)
            {
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            }
            _bangers = _content.Load<SpriteFont>("bangers");
        }

        public override void Unload()
        {
            _content.Unload();
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);
            PlayerIndex playerIndex;
            if (_startGame.Occurred(input, ControllingPlayer, out playerIndex))
            {
                MediaPlayer.Stop();
                this.IsExiting = true;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.DrawString(_bangers, "use WASD to move, the arrow keys to shoot,", new Vector2(100, _game.GraphicsDevice.Viewport.Height / 2 - 32), Color.Gold);
            spriteBatch.DrawString(_bangers, "and space to open doors", new Vector2(150, _game.GraphicsDevice.Viewport.Height / 2), Color.Gold);
            spriteBatch.DrawString(_bangers, "Press Enter to play", new Vector2(500, 450), Color.Gold);
            spriteBatch.End();

        }
    }
}
