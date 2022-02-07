using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace ELEPHANTSRPG.Objects
{
    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public class Player: WorldObject
    {
        public Direction Direction { get; private set; }
        public bool IsShooting { get; private set; } = false;
        private Texture2D texture;
        public Vector2 Position { get; private set; }
        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;

        public Player(Vector2 startPos)
        {
            Position = startPos;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/player");
        }

        public override void Update(GameTime gameTime)
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            IsShooting = false;

            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
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

            if(currentKeyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
            {
                IsShooting = true;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
