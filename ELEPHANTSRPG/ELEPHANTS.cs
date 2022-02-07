using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using ELEPHANTSRPG.Objects;
using ELEPHANTSRPG.Maps;

namespace ELEPHANTSRPG
{
    public class ELEPHANTS : Game
    {
        private GraphicsDeviceManager _graphics;
        private Textures textures;
        private SpriteBatch _spriteBatch;
        private List<WorldObject> objects;
        private Map map;

        public ELEPHANTS()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            map = new Map();
            objects = new List<WorldObject>();
            objects.Add(new Player(new Vector2(200, 200)));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            textures.LoadContent(Content);
            
            
            foreach (var worldObject in objects)
            {
                worldObject.LoadContent(Content);
            }
            map.LoadContent(Content);
            
            map.populateMap();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            foreach (var worldObject in objects)
            {
                worldObject.Update(gameTime);
            }
            Player player = (objects[0] as Player);
            if (player.IsShooting)
            {
                Bullet bullet = new Bullet(player.Direction, player.Position);
                objects.Add(bullet);
                bullet.LoadContent(Content);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            map.Draw(gameTime, _spriteBatch);
            foreach (var worldObject in objects)
            {
                worldObject.Draw(gameTime, _spriteBatch);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
