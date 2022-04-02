using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ELEPHANTSRPG.Collisions;
using ELEPHANTSRPG.Maps;

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
        private Tilemap _map;

        public Baddie(Vector2 position, Player player, Tilemap map)
        {
            Position = position;
            target = player;
            _map = map;
            _bounds = new BoundingRectangle(position + new Vector2(16,16), 32, 32);
            Random random = new Random();
            patrolDirection = new Vector2(1, random.Next(-1, 1));
            patrolDirection.Normalize();
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/ESkeleton");
        }

        public override void Update(GameTime gameTime)
        {
            Attacked = false;
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
                    if (_bounds.CollidesWith(target.Bounds))
                    {
                        Attacked = true;
                        target.Hit = true;
                        UpdatePosition(Position + newPosition * 100);
                        while (_map.CollidesWith(this))
                        {
                            Vector2 direction = Vector2.Normalize(Position - target.Position);
                            direction *= -1;
                            Position += direction * 10;
                            _bounds = new BoundingRectangle(Position + new Vector2(16, 16), 32, 32);
                        }
                    }
                    else
                    {
                        newPosition *= -1;
                        UpdatePosition(Position + newPosition * 75 * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    }
                }
                else
                {
                    patrolTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    UpdatePosition(Position + patrolDirection * 75 * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    if(patrolTime >= 2)
                    {
                        patrolDirection *= -1;
                        patrolTime = 0;
                    }
                }
            }

            if (_map.CollidesWith(this)) UndoUpdate();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsDead)
            {
                spriteBatch.Draw(texture, Position, new Rectangle(0, 0, 32, 32), Color.White);
            }
        }


        private void UpdatePosition(Vector2 newPosition)
        {
            Position = newPosition;
            _bounds.X = Position.X + 16;
            _bounds.Y = Position.Y + 16;
        }
        private void UndoUpdate()
        {
            Position = _priorPosition;
            _bounds = _priorBounds;
        }
    }
}
