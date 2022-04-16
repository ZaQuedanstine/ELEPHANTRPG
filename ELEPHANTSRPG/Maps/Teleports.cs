using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ELEPHANTSRPG.Objects;
using ELEPHANTSRPG.Collisions;
using Microsoft.Xna.Framework.Content;

namespace ELEPHANTSRPG.Maps
{
    public class Teleports: WorldObject
    {
        public Vector2 position;
        public Tilemap mapTarget;
        public bool Activated;
        private BoundingRectangle bounds;



        public Teleports(Vector2 pos, Tilemap target)
        {
            position = pos;
            mapTarget = target;
            bounds = new BoundingRectangle(position - new Vector2(5, 5), 64, 64);
        }

        public override BoundingRectangle Bounds => bounds;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //unnecessary part of map
        }

        public override void LoadContent(ContentManager content)
        {
            //Unnecessary
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
