using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ELEPHANTSRPG.Objects;

namespace ELEPHANTSRPG.Maps
{
    public class Map
    {
        private Texture2D grass;
        private Texture2D water;
        private int width;
        private int height;
        private int[,] theMap;
        private Tile[,] tiles;

        public Map()
        {
            width = 25;
            height = 15;
        }

        public void populateMap()
        {
            theMap = new int[width, height];
            for (int i = 2; i < width - 2; i++)
                for (int j = 2; j < height - 2; j++)
                    theMap[i, j] = 1;
            theMap[12, 7] = 0;
            theMap[13, 7] = 0;
            theMap[12, 8] = 0;
            theMap[13, 8] = 0;

            tiles = new Tile[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (theMap[i, j] == 0) tiles[i, j] = new Tile(water, true, new Vector2(i * 32, j * 32));
                    else tiles[i, j] = new Tile(grass, false, new Vector2(i * 32, j * 32));
                }
            }
        }


        public void LoadContent(ContentManager content)
        {
            grass = content.Load<Texture2D>("Sprites/grass");
            water = content.Load<Texture2D>("Sprites/water");
        }

        public bool Update(GameTime gameTIme, WorldObject thing)
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    if(tiles[i,j].IsSolid == true)
                    {
                        if (tiles[i, j].Bounds.CollidesWith(thing.Bounds))
                        {
                            return true;
                        }
                    }
                }
            return false;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Texture2D texture;
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    if (theMap[i, j] == 1)
                        texture = grass;
                    else texture = water;
                    tiles[i, j].Draw(gameTime, spriteBatch);
                }
        }
    }
}
