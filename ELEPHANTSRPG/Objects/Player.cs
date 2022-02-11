using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using ELEPHANTSRPG.Collisions;

namespace ELEPHANTSRPG.Objects
{
    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public class Player : WorldObject
    {
        public Direction Direction { get; private set; }
        public Direction ShootingDirection{ get; private set; }
        public bool IsShooting { get; private set; } = false;
        public Vector2 Position { get; private set; }
        public BoundingRectangle Bounds { get => bounds; }

        private Vector2 previousPosition;
        private BoundingRectangle bounds;
        private double shootTimer;
        private double shootTimeDelay;
        private Texture2D texture;
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        

        public Player(Vector2 startPos)
        {
            Position = startPos;
            shootTimeDelay = 0.3;
            bounds = new BoundingRectangle(startPos - new Vector2(-16,-16), 32, 32);
        }

        public override void LoadContent(Texture2D _texture)
        {
            texture = _texture;
        }

        public override void Update(GameTime gameTime)
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            shootTimer += gameTime.ElapsedGameTime.TotalSeconds;
            previousPosition = Position;
            IsShooting = false;

            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Movement
            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                Position += new Vector2(0, -75) * t;
                Direction = Direction.North;
            }
            if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                Position += new Vector2(0, 75) * t;
                Direction = Direction.South;
            }
            if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                Position += new Vector2(75, 0) * t;
                Direction = Direction.East;
            }
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                Position += new Vector2(-75, 0) * t;
                Direction = Direction.West;
            }

            //Shooting
            if (currentKeyboardState.IsKeyDown(Keys.Up) &&  shootTimer > shootTimeDelay)
            {
                if (currentKeyboardState.IsKeyUp(Keys.Down))
                {
                    shootTimer = 0;
                    IsShooting = true;
                    ShootingDirection = Direction.North;
                }
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right) && shootTimer > shootTimeDelay)
            {
                if (currentKeyboardState.IsKeyUp(Keys.Left))
                {
                    shootTimer = 0;
                    IsShooting = true;
                    ShootingDirection = Direction.East;
                }
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down) && shootTimer > shootTimeDelay)
            {
                if (currentKeyboardState.IsKeyUp(Keys.Up))
                {
                    shootTimer = 0;
                    IsShooting = true;
                    ShootingDirection = Direction.South;
                }
            }
            if (currentKeyboardState.IsKeyDown(Keys.Left) && shootTimer > shootTimeDelay)
            {
                if (currentKeyboardState.IsKeyUp(Keys.Right))
                {
                    shootTimer = 0;
                    IsShooting = true;
                    ShootingDirection = Direction.West;
                }
            }

            //Updates the bounds
            bounds.X = Position.X - 16;
            bounds.Y = Position.Y - 16;
        }

        public void UndoUpdate()
        {
            Position = previousPosition;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, new Rectangle(0, 0, 32, 32), Color.White);
        }
    }
}
