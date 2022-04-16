using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ELEPHANTSRPG.Objects;
using ELEPHANTSRPG.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace ELEPHANTSRPG.Maps
{
    public class Tilemap
    {
        public int TileWidth, TileHeight, MapWidth, MapHeight;
        Texture2D _tilesetTexture;
        Rectangle[] _tiles;
        public Vector2 PlayerStartPos;

        int[] _map;
        public string _fileName;

        public Tilemap(string fileName, Vector2 PlayerPos)
        {
            _fileName = fileName;
            PlayerStartPos = PlayerPos;
        }

        public void LoadContent(ContentManager content)
        {
            string data = File.ReadAllText(Path.Join(content.RootDirectory, _fileName));
            var lines = data.Split('\n');

            var tileSetFileName = lines[0].Trim();
            _tilesetTexture = content.Load<Texture2D>("Sprites/" + tileSetFileName);

            var secondLine = lines[1].Split(',');
            TileWidth = int.Parse(secondLine[0]);
            TileHeight = int.Parse(secondLine[1]);

            var tilesetColumns = _tilesetTexture.Width / TileWidth;
            var tilesetRows = _tilesetTexture.Height / TileHeight;
            _tiles = new Rectangle[tilesetColumns * tilesetRows];

            for(int y = 0; y < tilesetRows; y++)
            {
                for (int x = 0; x < tilesetColumns; x++)
                {
                    int index = y * tilesetColumns + x;
                    _tiles[index] = new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight);
                }
            }


            var thirdLine = lines[2].Split(',');
            MapWidth = int.Parse(thirdLine[0]);
            MapHeight = int.Parse(thirdLine[1]);


            var forthLine = lines[3].Split(',');

            _map = new int[MapWidth * MapHeight];
            for(int i = 0; i < MapWidth * MapHeight; i++)
            {
                _map[i] = int.Parse(forthLine[i]);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            for(int y = 0; y < MapHeight; y++)
            {
                for(int x = 0; x < MapWidth; x++)
                {
                    int index = _map[y * MapWidth + x] - 1;
                    if (index == -1) continue;
                    spritebatch.Draw(_tilesetTexture, new Vector2(x * TileWidth, y * TileHeight), _tiles[index], Color.White);
                }
            }
        }

        public bool CollidesWith(WorldObject thing)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    int index = y * MapWidth + x;
                    int maptile = _map[index];
                    if (maptile > 2)
                    {
                        BoundingRectangle bounds = new BoundingRectangle(new Vector2(x * TileWidth + 16, y * TileHeight + 16), 32, 32);
                        if (thing.Bounds.CollidesWith(bounds))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool CheckForCollisions(WorldObject thing, out int width, out int height)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    width = x * 32;
                    height = y * 32;
                    int index = y * MapWidth + x;
                    int maptile = _map[index];
                    if (maptile == 2 || maptile == 3)
                    {
                        BoundingRectangle bounds = new BoundingRectangle(new Vector2(x * TileWidth + 16, y * TileHeight + 16), 32, 32);
                        if (thing.Bounds.CollidesWith(bounds))
                        {
                            return true;
                        }
                    }
                }
            }
            width = 0;
            height = 0;
            return false;
        }
    }
}
