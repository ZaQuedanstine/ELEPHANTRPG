using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ELEPHANTSRPG.Collisions;

namespace ELEPHANTSRPG.Objects
{
    class TankBox : WorldObject
    {
        public override BoundingRectangle Bounds { get => _bounds; }
        public Vector2 Position;
        private BoundingRectangle _bounds;
        private Texture2D texture;
        private Player _player;
        public bool _collected = false;

        public TankBox(Vector2 position, Player player)
        {
            Position = position;
            _bounds = new BoundingRectangle(position, 32, 32);
            _player = player;
        }
        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/tankBox");
        }

        public override void Update(GameTime gameTime)
        {
            if (this.Bounds.CollidesWith(_player.Bounds) && _collected == false)
            {
                _collected = true;
                _player.TankEquipped = true;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!_collected) spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
