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
using Microsoft.Xna.Framework.Audio;
using ELEPHANTSRPG.Maps;
using ELEPHANTSRPG.Particle_System;
using ELEPHANTSRPG.Collisions;

namespace ELEPHANTSRPG.Screens
{
    public class GamePlayScreen: GameScreen
    {
        private List<WorldObject> bullets;
        private Player player;
        private Tilemap _map;
        private Tilemap world;
        private Tilemap home;
        private HUD hud;
        private SpriteFont bangers;
        private SpriteFont bangersBig;
        private Baddie[] baddies;
        private Teleports teleport;
        private ContentManager _content;
        private Song song;
        private SoundEffect shoot;
        private SoundEffect hurt;
        private SoundEffect hit;
        private bool gameOver = false;
        private bool firstBoot = true;
        private ExplosionParticleSystem explosions;
        private Game _game;
        private KeyboardState priorKeyboardState = new KeyboardState();
        Model tank;

        public GamePlayScreen(Game game)
        {
            _game = game;
        }

        private void LoadMap(Teleports teleporter)
        {
            player.Position = teleporter.mapTarget.PlayerStartPos;
            _map = teleporter.mapTarget;
            bullets = new List<WorldObject>();
            baddies = new Baddie[0];
            switch (teleport.mapTarget._fileName)
            {
                case "Maps/world.txt":
                    teleport = new Teleports(new Vector2(12 * 32, 17 * 32), home);
                    baddies = new Baddie[]
                    {
                        new Baddie(new Vector2(700,350), player, _map),
                        new Baddie(new Vector2(700,150), player, _map),
                        new Baddie(new Vector2(100,100), player, _map),
                        new Baddie(new Vector2(100,350), player, _map),
                        new Baddie(new Vector2(400,350), player, _map),
                        new Baddie(new Vector2(200,100), player, _map),

                        new Baddie(new Vector2(46 * _map.TileWidth, 6 * _map.TileHeight), player, _map),
                        new Baddie(new Vector2(46 * _map.TileWidth, 22 * _map.TileHeight), player, _map),
                        new Baddie(new Vector2(6 * _map.TileWidth, 45 * _map.TileHeight), player, _map),
                        new Baddie(new Vector2(38 * _map.TileWidth, 45 * _map.TileHeight), player, _map),
                        new Baddie(new Vector2(30 * _map.TileWidth, 25 * _map.TileHeight), player, _map),
                        new Baddie(new Vector2(17 * _map.TileWidth, 19 * _map.TileHeight), player, _map),

                    };
                    foreach (var baddie in baddies)
                    {
                        baddie.LoadContent(_content);
                    }
                    break;

                case "Maps/home.txt":
                    teleport = new Teleports(new Vector2(5 * 32, 11 * 32), world);
                    break;
            }
        }
        public override void Activate()
        {
            if (firstBoot)
            {
                explosions = new ExplosionParticleSystem(_game, 20);
                _game.Components.Add(explosions);
                firstBoot = false;
            }
            if (_content == null)
            {
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            tank = _content.Load<Model>("tank");
            world = new Tilemap("Maps/world.txt", new Vector2(12 * 32 + 1, 18 * 32 + 1));
            home = new Tilemap("Maps/home.txt", new Vector2(5 * 32, 11 * 32));
            world.LoadContent(_content);
            home.LoadContent(_content);
            _map = world;
            teleport = new Teleports(new Vector2(12 * 32, 17 * 32), home);
            player = new Player(_map.PlayerStartPos, _map);
            hud = new HUD(player);
            bullets = new List<WorldObject>();
            baddies = new Baddie[]
            {
                new Baddie(new Vector2(700,350), player, _map),
                new Baddie(new Vector2(700,150), player, _map),
                new Baddie(new Vector2(100,100), player, _map),
                new Baddie(new Vector2(100,350), player, _map),
                new Baddie(new Vector2(400,350), player, _map),
                new Baddie(new Vector2(200,100), player, _map),

                new Baddie(new Vector2(46 * _map.TileWidth, 6 * _map.TileHeight), player, _map),
                new Baddie(new Vector2(46 * _map.TileWidth, 22 * _map.TileHeight), player, _map),
                new Baddie(new Vector2(6 * _map.TileWidth, 45 * _map.TileHeight), player, _map),
                new Baddie(new Vector2(38 * _map.TileWidth, 45 * _map.TileHeight), player, _map),
                new Baddie(new Vector2(30 * _map.TileWidth, 25 * _map.TileHeight), player, _map),
                new Baddie(new Vector2(17 * _map.TileWidth, 19 * _map.TileHeight), player, _map),
            };
            bangers = _content.Load<SpriteFont>("bangers");
            bangersBig = _content.Load<SpriteFont>("bangersBig");

            player.LoadContent(_content);
            hud.LoadContent(_content);
            
            foreach (var baddie in baddies)
            {
                baddie.LoadContent(_content);
            }

            song = _content.Load<Song>("Music/CantinaRag");
            hurt =_content.Load<SoundEffect>("SFX/Hurt");
            shoot = _content.Load<SoundEffect>("SFX/Shoot");
            hit = _content.Load<SoundEffect>("SFX/Explosion");

        }

        public override void Unload()
        {
            _content.Unload();
        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if(this.IsActive)
            {
                UpdateGame(gameTime);
                if(MediaPlayer.State == MediaState.Stopped)
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(song);
                }
            }
        }

        public void UpdateGame(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            //update player
            if (!player.IsDead) player.Update(gameTime);

            if (_map.CollidesWith(player)) player.UndoUpdate();
            
            //update all the baddies
            foreach (var baddie in baddies)
            {
                baddie.Update(gameTime);
                if (baddie.Attacked == true) hit.Play();
            }

            //add any bullets the player is shooting
            if (player.IsShooting)
            {
                Bullet bullet = new Bullet(player.ShootingDirection, player.Position, _map.MapWidth * _map.TileWidth, _map.MapHeight * _map.TileHeight);
                bullet.LoadContent(_content);
                bullets.Add(bullet);
                shoot.Play();
            }

            //updates bullets and removes any that are not on the screen
            List<WorldObject> bulletsToRemove = new List<WorldObject>();
            foreach (var bullet in bullets)
            {
                bullet.Update(gameTime);
                foreach (var baddie in baddies)
                {
                    if (!baddie.IsDead && (bullet as Bullet).Bounds.CollidesWith(baddie.Bounds))
                    {
                        hurt.Play();
                        baddie.IsDead = true;
                        (bullet as Bullet).IsOffMap = true;
                        Vector2 explosionPosition = new Vector2();
                        explosionPosition.X = 384 - player.Position.X + baddie.Position.X;
                        explosionPosition.Y = 209 - player.Position.Y + baddie.Position.Y;
                        explosions.PlacedExplosion(explosionPosition);
                    }
                }
                if ((bullet as Bullet).IsOffMap)
                {
                    bulletsToRemove.Add(bullet);
                }

            }
            foreach (var bullet in bulletsToRemove)
            {
                bullets.Remove(bullet);
            }
            hud.Update(gameTime);


            if (gameOver)
            {
                var keyboardState = Keyboard.GetState();
                if(keyboardState.IsKeyDown(Keys.Enter))
                {
                    Unload();
                    Activate();
                    gameOver = false;
                }
            }

            //check teleports
            if (teleport.Bounds.CollidesWith(player.Bounds))
            {
                if (keyboard.IsKeyDown(Keys.Space) && priorKeyboardState.IsKeyUp(Keys.Space))
                    LoadMap(teleport);
            }
            priorKeyboardState = keyboard;
        }

        public override void Draw(GameTime gameTime)
        {
            float offsetx = 384 - player.Position.X;
            float offsety = 209 - player.Position.Y;
            Matrix transform = Matrix.CreateTranslation(offsetx, offsety, 0);
            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin(transformMatrix: transform);

            _map.Draw(gameTime, spriteBatch);
            player.Draw(gameTime, spriteBatch);
            foreach (var baddie in baddies)
            {
                baddie.Draw(gameTime, spriteBatch);
            }
            foreach (var bullet in bullets)
            {
                bullet.Draw(gameTime, spriteBatch);
            }

            hud.Draw(gameTime, spriteBatch, bangers);
            if (_map._fileName == "Maps/world.txt")
            {
                if (player.IsDead)
                {
                    gameOver = true;
                    spriteBatch.DrawString(bangersBig, "Game Over!!", player.Position - new Vector2(384, 50), Color.Red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                }
                int numOfBaddiesDead = 0;
                for (int i = 0; i < baddies.Length; i++)
                {
                    if (baddies[i].IsDead) numOfBaddiesDead++;
                }

                if (numOfBaddiesDead == baddies.Length)
                {
                    gameOver = true;
                    spriteBatch.DrawString(bangersBig, "You WIN!!", player.Position - new Vector2(384, 50), Color.Gold, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                }
            }
            if (gameOver) spriteBatch.DrawString(bangers, "Press Enter to play again", player.Position + new Vector2(50, 209), Color.Goldenrod, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);

            spriteBatch.End();

            //if(_map._fileName == "Maps/home.txt")
            //{
            float rotation = 0f;
            switch (player.Direction)
            {
                case ELEPHANTSRPG.Objects.Direction.North:
                    rotation = MathHelper.ToRadians(180);
                    break;
                case ELEPHANTSRPG.Objects.Direction.South:
                    rotation = MathHelper.ToRadians(0);
                    break;
                case ELEPHANTSRPG.Objects.Direction.East:
                    rotation = MathHelper.ToRadians(90);
                    break;
                case ELEPHANTSRPG.Objects.Direction.West:
                    rotation = MathHelper.ToRadians(-90);
                    break;
            }

                Matrix world = Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(new Vector3(10, 10, 0));
                Matrix view = Matrix.CreateLookAt(new Vector3(20, 30, 60), new Vector3(10, 10, 0), Vector3.Up);
                Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _game.GraphicsDevice.Viewport.AspectRatio, 1, 1000);
                
                tank.Draw(world, view, projection);
            //}
        }
    }
}
