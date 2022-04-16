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
        BasicEffect effect;
        Game game;
        Model chest;
        public House(Game game)
        {
            this.game = game;
        }

        public override void Activate()
        {
            if (_content == null)
            {
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            }
            _map = new Tilemap("Maps/home.txt", new Vector2(5 * 32, 11 * 32));
            _map.LoadContent(_content);
            _player = new Player(_map.PlayerStartPos, _map);
            _player.LoadContent(_content);
            chest = _content.Load<Model>("tank");
            effect = new BasicEffect(game.GraphicsDevice);
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
            Matrix world = Matrix.CreateRotationY(0f) * Matrix.CreateTranslation(new Vector3(10, 10, 0));
            Matrix view = Matrix.CreateLookAt(new Vector3(20, 30, 20), new Vector3(10, 10, 0), Vector3.Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.GraphicsDevice.Viewport.AspectRatio, 1, 1000);
            
            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            _map.Draw(gameTime, spriteBatch);
            _player.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            chest.Draw(world, view, projection);
        }

    }
}
