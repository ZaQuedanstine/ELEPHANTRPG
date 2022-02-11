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
        public bool IsOffScreen;

        const float velocity = 200;
        private Texture2D texture;
        private Direction direction;
        private Vector2 position;
        private int maxX;
        private int maxY;
        private float angle;

        public Bullet(Direction bulletDirection, Vector2 initialPosition, GraphicsDevice graphics)
        {
            direction = bulletDirection;
            position = initialPosition;
            maxX = graphics.Viewport.Width;
            maxY = graphics.Viewport.Height;
            switch(direction)
            {
                case Direction.East:
                    angle = 0;
                    break;
                case Direction.North:
                    angle = -90;
                    break;
                case Direction.West:
                    angle = 180;
                    break;
                case Direction.South:
                    angle = 90;
                    break;
            }
            angle = MathHelper.ToRadians(angle);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, angle, new Vector2(16, 16), 1, SpriteEffects.None, 0);
        }

        public override void LoadContent(Texture2D _texture)
        {
            texture = _texture;
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

            if (position.X < -32 || position.X > maxX || position.Y < -32 || position.Y > maxY) IsOffScreen = true;
        }
    }
}
