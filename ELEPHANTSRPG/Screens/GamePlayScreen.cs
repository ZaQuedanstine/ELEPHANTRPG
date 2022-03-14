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

namespace ELEPHANTSRPG.Screens
{
    public class GamePlayScreen: GameScreen
    {
        private List<WorldObject> bullets;
        private Player player;
        private Tilemap _map;
        private HUD hud;
        private SpriteFont bangers;
        private SpriteFont bangersBig;
        private Baddie[] baddies;
        private ContentManager _content;
        private Song song;
        private SoundEffect shoot;
        private SoundEffect hurt;
        private SoundEffect hit;
        private bool gameOver = false;
        private bool firstBoot = true;
        private ExplosionParticleSystem explosions;
        private Game _game;

        public GamePlayScreen(Game game)
        {
            _game = game;
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

            _map = new Tilemap("Maps/startMap.txt");
            player = new Player(new Vector2(200, 200));
            hud = new HUD(player);
            bullets = new List<WorldObject>();
            baddies = new Baddie[]
            {
                new Baddie(new Vector2(700,350), player),
                new Baddie(new Vector2(700,150), player),
                new Baddie(new Vector2(100,100), player),
                new Baddie(new Vector2(100,350), player),
                new Baddie(new Vector2(400,350), player),
                new Baddie(new Vector2(200,100), player),
            };
            bangers = _content.Load<SpriteFont>("bangers");
            bangersBig = _content.Load<SpriteFont>("bangersBig");

            player.LoadContent(_content);
            hud.LoadContent(_content);
            _map.LoadContent(_content);
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
            //update player
            if (!player.IsDead) player.Update(gameTime);
            foreach (var baddie in baddies)
            {
                if (!baddie.IsDead && baddie.Bounds.CollidesWith(player.Bounds))
                {
                    hit.Play();
                    player.Hit = true;
                    baddie.Attacked = true;
                    
                }
                baddie.Update(gameTime);
                if(_map.CheckForCollisions(baddie))
                {
                    baddie.UndoUpdate();
                }
            }
            if (_map.CheckForCollisions(player) == true)
            {
                player.UndoUpdate();
            }

            //addd any bullets the player is shooting
            if (player.IsShooting)
            {
                Bullet bullet = new Bullet(player.ShootingDirection, player.Position);
                bullet.LoadContent(_content);
                bullets.Add(bullet);
                shoot.Play();
            }

            //updates bullets and removes any that are not on the screen
            //checks for collisions with baddies and kills them on contact
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
                        (bullet as Bullet).IsOffScreen = true;
                        explosions.PlacedExplosion(baddie.Position);
                    }
                }
                if ((bullet as Bullet).IsOffScreen)
                {
                    bulletsToRemove.Add(bullet);
                }

            }
            foreach (var bullet in bulletsToRemove)
            {
                bullets.Remove(bullet);
            }
            hud.Update(gameTime);


            if(gameOver)
            {
                var keyboardState = Keyboard.GetState();
                if(keyboardState.IsKeyDown(Keys.Enter))
                {
                    Unload();
                    Activate();
                    gameOver = false;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();

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
            if (player.IsDead)
            {
                gameOver = true;
                spriteBatch.DrawString(bangersBig, "Game Over!!", new Vector2(25, 100), Color.Red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            }
            int numOfBaddiesDead = 0;
            for (int i = 0; i < baddies.Length; i++)
            {
                if (baddies[i].IsDead) numOfBaddiesDead++;
            }
            if (numOfBaddiesDead == baddies.Length)
            {
                gameOver = true;
                spriteBatch.DrawString(bangersBig, "You WIN!!", new Vector2(75, 100), Color.Gold, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            }
            if (gameOver) spriteBatch.DrawString(bangers, "Press Enter to play again", new Vector2(500, 400), Color.Goldenrod, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
