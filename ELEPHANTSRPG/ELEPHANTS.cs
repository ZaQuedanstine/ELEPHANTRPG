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
        private List<WorldObject> bullets;
        private Player player;
        private Map map;

        public ELEPHANTS()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        protected override void Initialize()
        {
            map = new Map();
            player = new Player(new Vector2(200, 200));
            bullets = new List<WorldObject>();
            textures = new Textures();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            textures.LoadContent(Content);
            player.LoadContent(textures.Player);
            map.LoadContent(Content);
            
            map.populateMap();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //update player
            player.Update(gameTime);
            map.Update(gameTime, player);
            if(map.PlayerIsCollidingWithSolidTile)
            {
                player.UndoUpdate();
            }

            //addd any bullets the player is shooting
            if (player.IsShooting && textures.Bullet != null)
            {
                Bullet bullet = new Bullet(player.ShootingDirection, player.Position, GraphicsDevice);
                bullet.LoadContent(textures.Bullet);
                bullets.Add(bullet);
                
            }
            //updates bullets and removes any that are not on the screen
            List<WorldObject> bulletsToRemove = new List<WorldObject>();
            foreach (var bullet  in bullets)
            {
                
                bullet.Update(gameTime);
                if((bullet as Bullet).IsOffScreen)
                {
                    bulletsToRemove.Add(bullet);
                }
            }
            foreach (var bullet in bulletsToRemove)
            {
                bullets.Remove(bullet);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            map.Draw(gameTime, _spriteBatch);
            player.Draw(gameTime, _spriteBatch);
            foreach (var bullet in bullets)
            {
                bullet.Draw(gameTime, _spriteBatch);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
