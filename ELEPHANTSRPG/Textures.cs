using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ELEPHANTSRPG
{
    public class Textures
    {
        public Texture2D Player;
        public Texture2D Bullet;
        public Texture2D Grass;
        public Texture2D Water;

        public void LoadContent(ContentManager content)
        {
            Player = content.Load<Texture2D>("Sprites/player");
            Bullet = content.Load<Texture2D>("Sprites/bullet");
            Grass = content.Load<Texture2D>("Sprites/grass");
            Water = content.Load<Texture2D>("Sprites/water");
        }
    }
}
