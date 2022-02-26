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
    public class TitleScreen: GameScreen
    {
        private ContentManager _content;
        private Banner _banner;
        private Clouds _clouds;
        private Elephant _elephant;
        private Ground _ground;
        private SpriteFont _bangers;
        private Song song;

        private readonly InputAction _startGame;
        public TitleScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _startGame = new InputAction(
                new[] { Buttons.Start },
                new[] { Keys.Enter, Keys.Space },
                true);
        }

        public override void Activate()
        {
            

            if(_content == null)
            {
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            }
            _bangers = _content.Load<SpriteFont>("bangers");
            _banner = new Banner();
            _clouds = new Clouds();
            _elephant = new Elephant();
            _ground = new Ground();
            _ground.LoadContent(_content);
            _elephant.LoadContent(_content);
            _banner.LoadContent(_content);
            _clouds.LoadContent(_content);

            song = _content.Load<Song>("Music/Buckbreak");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
        }

        public override void Unload()
        {
            _content.Unload();
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);
            PlayerIndex playerIndex;
            if(_startGame.Occurred(input, ControllingPlayer, out playerIndex))
            {
                MediaPlayer.Stop();
                this.IsExiting = true;
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            _banner.Update(gameTime);
            _clouds.Update(gameTime);
            _elephant.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
                _clouds.Draw(gameTime, spriteBatch);
                _ground.Draw(gameTime, spriteBatch);
                _elephant.Draw(gameTime, spriteBatch);
                _banner.Draw(gameTime, spriteBatch);
                spriteBatch.DrawString(_bangers, "Press Enter to play", new Vector2(500, 450), Color.Gold);
            spriteBatch.End();

        }
    }
}
