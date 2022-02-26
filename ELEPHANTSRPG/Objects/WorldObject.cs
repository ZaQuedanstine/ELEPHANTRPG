using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace ELEPHANTSRPG.Objects
{
    /// <summary>
    /// An interface representing an object in the game world
    /// </summary>
    public abstract class WorldObject 
    {
        /// <summary>
        /// The Load Content method of the world object for loading textures
        /// </summary>
        /// <param name="content">the content manager of the game that needs to load the content</param>
        public abstract void LoadContent(ContentManager content);

        /// <summary>
        /// The Update method to make the object "do" stuff
        /// </summary>
        /// <param name="gameTime">uses the time to determine how much stuff should move as a function of the amount of time that has passed</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draws this object to the game world so we can see it
        /// </summary>
        /// <param name="gameTime">used for time calculations</param>
        /// <param name="spriteBatch">the spritebatch that is drawing this object</param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
