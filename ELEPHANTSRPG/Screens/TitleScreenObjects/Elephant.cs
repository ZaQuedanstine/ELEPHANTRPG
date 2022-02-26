using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ELEPHANTSRPG.Screens.TitleScreenObjects
{
    public enum Direction
    {
        Forward,
        Right,
        Left,
        Backward
    }

    public class Elephant
    {
        private Texture2D texture;
        private Texture2D gun;
        private double animationTimer;
        private double directionTimer;
        private short animationFrame;
        private Direction direction = Direction.Right;
        private Vector2 position;

        public Elephant()
        {
            position = new Vector2(800 / 2, 450 - 48);
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/player");
            gun = content.Load<Texture2D>("Sprites/pack");
        }

        public void Update(GameTime gameTime)
        {
            directionTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (directionTimer > 3.0)
            {
                switch (direction)
                {
                    case Direction.Right:
                        direction = Direction.Left;
                        break;
                    case Direction.Left:
                        direction = Direction.Forward;
                        break;
                    case Direction.Forward:
                        direction = Direction.Right;
                        break;

                }
                directionTimer = 0;
            }

            if (direction == Direction.Right)
            {
                position += new Vector2(1, 0);
            }
            if (direction == Direction.Left)
            {
                position += new Vector2(-1, 0);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > 0.2)
            {
                animationFrame++;
                if (animationFrame > 2)
                {
                    animationFrame = 0;
                }
                animationTimer = 0;
            }
            Rectangle source;
            Rectangle gunSource = new Rectangle(0, 96, 16, 16);

            source = new Rectangle(animationFrame * 32, (int)direction * 32, 32, 32);

            spriteBatch.Draw(texture, position, source, Color.White, 0, new Vector2(8, 8), 2f, SpriteEffects.None, 0);

            SpriteEffects spriteEffects = (direction == Direction.Left) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(gun, position + new Vector2(12,8), gunSource, Color.White, 0, new Vector2(8, 8), 3f, spriteEffects, 0);
        }
    }
}
