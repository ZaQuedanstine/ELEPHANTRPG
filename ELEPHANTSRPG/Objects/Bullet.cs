using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELEPHANTSRPG.Objects
{
    public class Bullet: WorldObject
    {
        const float velocity = 200;
        private Texture2D texture;
        private Direction direction;
        private Vector2 position;

        public Bullet(Direction bulletDirection, Vector2 initialPosition)
        {
            direction = bulletDirection;
            position = initialPosition;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/bullet");
        }

        public override void Update(GameTime gameTime)
        {
            switch(direction)
            {
                case Direction.North:
                    position += new Vector2(0, -velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    break;
                case Direction.East:
                    position += new Vector2(velocity * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                    break;
                case Direction.South:
                    position += new Vector2(0, velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    break;
                case Direction.West:
                    position += new Vector2(-velocity * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                    break;

            }
        }
    }
}
