using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ELEPHANTSRPG.Screens.TitleScreenObjects
{
    public class Ground
    {
        private Texture2D texture;
        private float width;
        private float height;

        public Ground()
        {
            width = 800 - 32;
            height = 450;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/grass_32x32");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float currentWidth = width;
            while (currentWidth >= 0)
            {
                spriteBatch.Draw(texture, new Vector2(currentWidth, height), Color.White);
                currentWidth -= 32;
            }
        }
    }
}
