using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ELEPHANTSRPG.Screens.TitleScreenObjects
{
    public class Banner
    {
        private Texture2D texture;
        private Vector2 position;
        private float height;
        private bool isFalling = true;
        private int numOfBounces = 0;

        public Banner()
        {
            float width = 800;
            height = 450;
            position = new Vector2(width / 2, -200);
        }
        public void Update(GameTime gameTime)
        {
            if (position.Y >= height / 2)
            {
                isFalling = false;
            }
            if (isFalling)
            {
                switch (numOfBounces)
                {
                    case 0:
                        position += new Vector2(0, 10);
                        break;
                    case 1:
                        position += new Vector2(0, 8);
                        break;
                    case 2:
                        position += new Vector2(0, 5);
                        break;
                }

            }
            else if (numOfBounces < 3)
            {
                switch (numOfBounces)
                {
                    case 0:
                        position += new Vector2(0, -8);
                        if (position.Y <= 0)
                        {
                            isFalling = true;
                            numOfBounces++;
                        }
                        break;
                    case 1:
                        position += new Vector2(0, -5);
                        if (position.Y <= 100)
                        {
                            isFalling = true;
                            numOfBounces++;
                        }
                        break;
                    case 2:
                        position += new Vector2(0, -3);
                        if (position.Y <= 150)
                        {
                            isFalling = true;
                            numOfBounces++;
                        }
                        break;
                }

            }
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/Banner");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position + new Vector2(-289, -150), Color.White);
        }
    }
}
