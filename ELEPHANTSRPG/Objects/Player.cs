using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using ELEPHANTSRPG.Collisions;
using ELEPHANTSRPG.Maps;

namespace ELEPHANTSRPG.Objects
{
    public enum Direction
    {
        South,
        East,
        West,
        North
    }

    public enum HealthStatus
    {
        Full,
        Three_Quarters,
        Half,
        One_Quarter,
        Empty
    }

    public class Player : WorldObject
    {
        public Direction Direction { get; private set; }
        public Direction ShootingDirection{ get; private set; }
        public bool IsShooting { get; private set; } = false;
        public Vector2 Position { get; set; }
        public override BoundingRectangle Bounds { get => bounds; }

        public bool Hit;
        public bool IsDead;
        public int TotalNumOfPeanuts = 3;
        public int CurrentPeanut = 3;
        public HealthStatus LifeLeftOnCurrentPeanut = HealthStatus.Full;

        private Vector2 previousPosition;
        private BoundingRectangle bounds;
        private double shootTimer;
        private double shootTimeDelay;
        private Texture2D texture;
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private double animationTimer;
        private Rectangle rectangle = new Rectangle(0,0,32,32);
        public Tilemap _map;


        public Player(Vector2 startPos, Tilemap map)
        {
            Position = startPos;
            shootTimeDelay = 0.4;
            bounds = new BoundingRectangle(startPos.X + 16, startPos.Y + 16, 20, 32);
            _map = map;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/player");
        }

        public override void Update(GameTime gameTime)
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            shootTimer += gameTime.ElapsedGameTime.TotalSeconds;
            previousPosition = Position;
            IsShooting = false;

            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Health Stuff
            if(Hit)
            {
                if (LifeLeftOnCurrentPeanut == HealthStatus.Empty)
                {
                    LifeLeftOnCurrentPeanut = HealthStatus.Full;
                    CurrentPeanut--;
                }
                else LifeLeftOnCurrentPeanut++;
                if (CurrentPeanut == 1 && LifeLeftOnCurrentPeanut == HealthStatus.Empty) IsDead = true;
                Hit = false;
            }

            //Movement
            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                Position += new Vector2(0, -100) * t;
                Direction = Direction.North;
            }
            if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                Position += new Vector2(0, 100) * t;
                Direction = Direction.South;
            }
            if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                Position += new Vector2(100, 0) * t;
                Direction = Direction.East;
            }
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                Position += new Vector2(-100, 0) * t;
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

            if (IsShooting) rectangle.X = 96;

            //Updates the bounds
            bounds.X = Position.X + 16;
            bounds.Y = Position.Y + 16;
        }

        public void UndoUpdate()
        {
            Position = previousPosition;
            bounds.X = Position.X + 16;
            bounds.Y = Position.Y + 16;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if(animationTimer >= 0.3)
            {
                animationTimer = 0;
                if (rectangle.X < 64) rectangle.X += 32;
                else rectangle.X = 0;
                rectangle.Y = 32 * (int)Direction;
            }
            if(!IsDead)
            spriteBatch.Draw(texture, Position, rectangle, Color.White);
        }
    }
}
