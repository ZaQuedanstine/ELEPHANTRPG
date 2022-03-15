using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using ELEPHANTSRPG.Collisions;

namespace ELEPHANTSRPG.Objects
{
    public class Bullet: WorldObject
    {
        public bool IsOffMap;
        public override BoundingRectangle Bounds { get => bounds; }

        private BoundingRectangle bounds;
        const float velocity = 250;
        private Texture2D texture;
        private Direction direction;
        public Vector2 Position;
        private int maxX;
        private int maxY;
        private float angle;

        public Bullet(Direction bulletDirection, Vector2 initialPosition, int maxRightDistance, int maxDownDistance)
        {
            direction = bulletDirection;
            Position = initialPosition;
            Position.X += 16;
            Position.Y += 8;
            maxX = maxRightDistance;
            maxY = maxDownDistance;
            bounds = new BoundingRectangle(Position.X + 16, Position.Y + 16, 4, 8);
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
            spriteBatch.Draw(texture, Position, null, Color.White, angle, new Vector2(16, 16), 1, SpriteEffects.None, 0);
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
                    Position += new Vector2(0, -velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    break;
                case Direction.East:
                    Position += new Vector2(velocity * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                    break;
                case Direction.South:
                    Position += new Vector2(0, velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    break;
                case Direction.West:
                    Position += new Vector2(-velocity * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                    break;

            }
            bounds.X = Position.X + 16;
            bounds.Y = Position.Y + 16;
            if (Position.X < -32 || Position.X > maxX || Position.Y < -32 || Position.Y > maxY) IsOffMap = true;
        }
    }
}
