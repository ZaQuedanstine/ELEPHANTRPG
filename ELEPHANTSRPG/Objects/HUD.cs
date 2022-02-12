using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELEPHANTSRPG.Objects
{
    public class HUD
    {
        Texture2D peanuts;
        Player player;
        int totalPeanuts;
        int currentPeanut;
        HealthStatus lifeLeft;
        public HUD(Player thePlayer)
        {
            player = thePlayer;
        }
        public void LoadContent(Texture2D texture)
        {
            peanuts = texture;
        }

        public void Update(GameTime gameTime)
        {
            totalPeanuts = player.TotalNumOfPeanuts;
            lifeLeft = player.LifeLeftOnCurrentPeanut;
            currentPeanut = player.CurrentPeanut;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, "Life: ", new Vector2(10, 30), Color.White);
            for(int i = 0; i < totalPeanuts; i++ )
            {
                
                if(i == currentPeanut - 1) spriteBatch.Draw(peanuts, new Vector2(75 + (i * 37), 30), new Rectangle(0 + (32 * (int)lifeLeft), 0, 32, 32), Color.White);
                if(i > currentPeanut - 1) spriteBatch.Draw(peanuts, new Vector2(75 + (i * 37), 30), new Rectangle(0 + (32 * 4), 0, 32, 32), Color.White);
                else if(i < currentPeanut - 1) spriteBatch.Draw(peanuts, new Vector2(75 + (i * 37), 30), new Rectangle(0, 0, 32, 32), Color.White);
            }
        }
    }
}
