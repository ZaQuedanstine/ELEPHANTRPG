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
        int _tileWidth, _tileHeight, _mapWidth, _mapHeight;
        Texture2D _tilesetTexture;
        Rectangle[] _tiles;
        
        int[] _map;
        string _fileName;

        public Tilemap(string fileName)
        {
            _fileName = fileName;
        }

        public void LoadContent(ContentManager content)
        {
            string data = File.ReadAllText(Path.Join(content.RootDirectory, _fileName));
            var lines = data.Split('\n');

            var tileSetFileName = lines[0].Trim();
            _tilesetTexture = content.Load<Texture2D>("Sprites/" + tileSetFileName);

            var secondLine = lines[1].Split(',');
            _tileWidth = int.Parse(secondLine[0]);
            _tileHeight = int.Parse(secondLine[1]);

            var tilesetColumns = _tilesetTexture.Width / _tileWidth;
            var tilesetRows = _tilesetTexture.Height / _tileHeight;
            _tiles = new Rectangle[tilesetColumns * tilesetRows];

            for(int y = 0; y < tilesetColumns; y++)
            {
                for (int x = 0; x < tilesetRows; x++)
                {
                    int index = y * tilesetColumns + x;
                    _tiles[index] = new Rectangle(x * _tileWidth, y * _tileHeight, _tileWidth, _tileHeight);
                }
            }


            var thirdLine = lines[2].Split(',');
            _mapWidth = int.Parse(thirdLine[0]);
            _mapHeight = int.Parse(thirdLine[1]);


            var forthLine = lines[3].Split(',');

            _map = new int[_mapWidth * _mapHeight];
            for(int i = 0; i < _mapWidth * _mapHeight; i++)
            {
                _map[i] = int.Parse(forthLine[i]);
            }
        }

        public bool CheckForCollisions(WorldObject thing)
        {
            for(int y = 0; y < _mapHeight; y++)
            {
                for(int x = 0; x < _mapWidth; x++)
                {
                    int index = y * _mapWidth + x;
                    int maptile = _map[index];
                    if(maptile == 2 || maptile == 3)
                    {
                        BoundingRectangle bounds = new BoundingRectangle(new Vector2(x * _tileWidth + 16, y * _tileHeight + 16), 32, 32);
                        if(thing.Bounds.CollidesWith(bounds))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            for(int y = 0; y < _mapHeight; y++)
            {
                for(int x = 0; x < _mapWidth; x++)
                {
                    int index = _map[y * _mapWidth + x] - 1;
                    if (index == -1) continue;
                    spritebatch.Draw(_tilesetTexture, new Vector2(x * _tileWidth, y * _tileHeight), _tiles[index], Color.White);
                }
            }
        }
    }
}
