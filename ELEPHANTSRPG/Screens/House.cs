using System;
using System.Collections.Generic;
using System.Text;
using ELEPHANTSRPG.StateManagement;
using ELEPHANTSRPG.Maps;
using ELEPHANTSRPG.Objects;
using ELEPHANTSRPG.Screens.TitleScreenObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ELEPHANTSRPG.Screens
{
    public class House: GameScreen
    {
        private Tilemap _map;
        private ContentManager _content;
        private Player _player;

        public House()
        {

        }

        public override void Activate()
        {
            if (_content == null)
            {
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            }
            _map = new Tilemap("Maps/home.txt");
            _map.LoadContent(_content);
            _player = new Player(new Vector2(50, 100), _map);
            _player.LoadContent(_content);
        }

        public override void Unload()
        {
            _content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            _player.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            _map.Draw(gameTime, spriteBatch);
            _player.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

    }
}
