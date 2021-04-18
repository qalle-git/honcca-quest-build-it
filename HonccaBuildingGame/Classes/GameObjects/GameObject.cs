using HonccaBuildingGame.Classes.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HonccaBuildingGame.Classes.GameObjects
{
    abstract class GameObject
    {
        public Texture2D Texture;

        public Vector2 Position;
        public Vector2 Momentum;

        public bool PhysicsEnabled;

        public enum Axis
        {
            SIDE,
            UP,
            DOWN
        }
        
        public GameObject(Vector2 startPosition, Texture2D startTexture)
        {
            Position = startPosition;

            Texture = startTexture;
        }

        /// <summary>
        /// Get the hitbox rectangle from the gameobject.
        /// </summary>
        /// <returns>A rectangle containing the hitbox.</returns>
        public virtual Rectangle GetRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        /// <summary>
        /// This will be called each frame and should be used to update physics, input etc.
        /// </summary>
        /// <param name="gameTime">The current gametime object.</param>
        public virtual void Update(GameTime gameTime)
        {
            if (PhysicsEnabled)
            {
                Physics(gameTime);
            } 
            else
            {
                Position += Momentum * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (Position.Y > 1000)
            {
                Position.Y = 1000;
                Momentum = Vector2.Zero;
            }
        }

        /// <summary>
        /// This will be called each frame and should be used to draw character etc.
        /// </summary>
        /// <param name="gameTime">The current gametime object.</param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        /// <summary>
        /// This handles the physics, makes gravity and collision work.
        /// </summary>
        /// <param name="gameTime">The current gametime object.</param>
        public virtual void Physics(GameTime gameTime)
        {
            Momentum.Y += 1000 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 frameMovement = Momentum * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!Globals.TheTileMap.CanFall(GetRectangle(), ref frameMovement))
            {
                Momentum.Y = 0;
            }

            Position += frameMovement;
        }
    }
}
