using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ELEPHANTSRPG.Collisions;

namespace ELEPHANTSRPG.Objects
{
    public class Baddie : WorldObject
    {
        private Texture2D texture;
        private Player target;

        public Vector2 Position;
        public BoundingRectangle Bounds;
        public bool Attacked;
        public bool IsDead;


        public Baddie(Vector2 position, Player player)
        {
            Position = position;
            target = player;
            Bounds = new BoundingRectangle(position + new Vector2(16,16), 32, 32);
        }

        public override void LoadContent(Texture2D _texture)
        {
            texture = _texture;
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsDead)
            {
                Vector2 newPosition = Vector2.Normalize(Position - target.Position);
                if (Attacked)
                {
                    Position += newPosition * 100;
                    Attacked = false;
                }
                else
                {
                    newPosition *= -1;
                    Position += newPosition * 75 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                Bounds.X = Position.X + 16;
                Bounds.Y = Position.Y + 16;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsDead)
            {
                spriteBatch.Draw(texture, Position, Color.White);
            }
        }
    }
}
