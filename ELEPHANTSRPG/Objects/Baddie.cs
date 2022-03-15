using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ELEPHANTSRPG.Collisions;

namespace ELEPHANTSRPG.Objects
{
    public class Baddie : WorldObject
    {
        private Texture2D texture;
        private Player target;

        public Vector2 Position;
        private Vector2 _priorPosition;
        public override BoundingRectangle Bounds { get => _bounds; }
        private BoundingRectangle _bounds;
        private BoundingRectangle _priorBounds;
        public bool Attacked;
        public bool IsDead;
        private bool _enemyDetected;
        private float patrolTime;
        private Vector2 patrolDirection;


        public Baddie(Vector2 position, Player player)
        {
            Position = position;
            target = player;
            _bounds = new BoundingRectangle(position + new Vector2(16,16), 32, 32);
            Random random = new Random();
            patrolDirection = new Vector2(1, random.Next(-1, 1));
            patrolDirection.Normalize();
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/baddie");
        }

        public override void Update(GameTime gameTime)
        {
            _priorPosition = Position;
            _priorBounds = _bounds;

            if (!IsDead)
            {
                Vector2 distance = Position - target.Position;
                if (Math.Sqrt(Math.Pow(distance.X, 2) + Math.Pow(distance.Y, 2)) < 350) _enemyDetected = true;
                else _enemyDetected = false;
                if (_enemyDetected)
                {
                    Vector2 newPosition = Vector2.Normalize(distance);
                    if (Attacked)
                    {
                        Position += newPosition * 100;
                        Attacked = false;
                    }
                    else
                    {
                        newPosition *= -1;
                        Position += newPosition * 75 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }
                else
                {
                    patrolTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Position += patrolDirection * 75 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if(patrolTime >= 2)
                    {
                        patrolDirection *= -1;
                        patrolTime = 0;
                    }
                }
                
            }

            _bounds.X = Position.X + 16;
            _bounds.Y = Position.Y + 16;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsDead)
            {
                spriteBatch.Draw(texture, Position, Color.White);
            }
        }

        public void UndoUpdate()
        {
            Position = _priorPosition;
            _bounds = _priorBounds;
        }

        public void updateBounds(BoundingRectangle rectangle)
        {
            _bounds = rectangle;
        }
    }
}
