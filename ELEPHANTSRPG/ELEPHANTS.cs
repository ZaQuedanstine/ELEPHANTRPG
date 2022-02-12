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
        private HUD hud;
        private SpriteFont bangers;
        private Baddie[] baddies;

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
            hud = new HUD(player);
            bullets = new List<WorldObject>();
            textures = new Textures();
            baddies = new Baddie[]
            {
                new Baddie(new Vector2(700,350), player),
                new Baddie(new Vector2(700,100), player),
                new Baddie(new Vector2(100,100), player),
                new Baddie(new Vector2(100,350), player),
                new Baddie(new Vector2(400,350), player),
                new Baddie(new Vector2(200,50), player),
            };
            bangers = Content.Load<SpriteFont>("bangers");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            textures.LoadContent(Content);
            player.LoadContent(textures.Player);
            hud.LoadContent(textures.Health);
            map.LoadContent(Content);
            foreach(var baddie in baddies)
            {
                baddie.LoadContent(textures.Baddie);
            }
            map.populateMap();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //update player
            if(!player.IsDead) player.Update(gameTime);
            foreach (var baddie in baddies)
            {
                if (!baddie.IsDead && baddie.Bounds.CollidesWith(player.Bounds))
                {
                    player.Hit = true;
                    baddie.Attacked = true;
                }
                baddie.Update(gameTime);
            }
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
            //checks for collisions with baddies and kills them on contact
            List<WorldObject> bulletsToRemove = new List<WorldObject>();
            foreach (var bullet  in bullets)
            {
                bullet.Update(gameTime);
                foreach (var baddie in baddies)
                {
                    if ((bullet as Bullet).Bounds.CollidesWith(baddie.Bounds))
                    {

                        baddie.IsDead = true;
                        (bullet as Bullet).IsOffScreen = true;
                    }
                }
                if((bullet as Bullet).IsOffScreen)
                {
                    bulletsToRemove.Add(bullet);
                }
                
            }
            foreach (var bullet in bulletsToRemove)
            {
                bullets.Remove(bullet);
            }
            
            
            
            hud.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            map.Draw(gameTime, _spriteBatch);
            player.Draw(gameTime, _spriteBatch);
            foreach (var baddie in baddies)
            {
                baddie.Draw(gameTime, _spriteBatch);
            }
            foreach (var bullet in bullets)
            {
                bullet.Draw(gameTime, _spriteBatch);
            }
            
            hud.Draw(gameTime, _spriteBatch, bangers);
            if (player.IsDead) _spriteBatch.DrawString(bangers, "Game Over!!", new Vector2(100, 100), Color.Red, 0f, new Vector2(0, 0), 5f, SpriteEffects.None, 0);
            int numOfBaddiesDead = 0;
            for(int i = 0; i < baddies.Length; i++)
            {
                if (baddies[i].IsDead) numOfBaddiesDead++;
            }
            if(numOfBaddiesDead == baddies.Length) _spriteBatch.DrawString(bangers, "You WIN!!", new Vector2(100, 100), Color.Gold, 0f, new Vector2(0, 0), 5f, SpriteEffects.None, 0);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
