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

        public bool IsSolid { get; private set; }

        public Tile(Texture2D theTexture, bool isSolid)
        {
            texture = theTexture;
            IsSolid = isSolid;
        }

        public void Update(GameTime gameTime)
        {
            if(IsSolid)
            {

            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
