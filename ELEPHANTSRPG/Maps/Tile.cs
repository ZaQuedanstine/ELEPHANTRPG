using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ELEPHANTSRPG.Collisions;

namespace ELEPHANTSRPG.Maps
{
    public class Tile
    {
        private Texture2D texture;
        private BoundingRectangle bounds;

        public BoundingRectangle Bounds { get => bounds; }
        public Vector2 Position;

        public bool IsSolid { get; private set; }

        public Tile(Texture2D theTexture, bool isSolid, Vector2 position)
        {
            texture = theTexture;
            IsSolid = isSolid;
            Position = position;
            bounds = new BoundingRectangle(position - new Vector2(16, 16), 32, 32);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
