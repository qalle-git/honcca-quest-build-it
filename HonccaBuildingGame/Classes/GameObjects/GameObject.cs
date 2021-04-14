using HonccaBuildingGame.Classes.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonccaBuildingGame.Classes.GameObjects
{
    abstract class GameObject
    {
        public Texture2D Texture;

        public Vector2 Position;
        public Vector2 Momentum;

        public bool PhysicsEnabled;
        public bool Active = true;

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

        public virtual Rectangle GetRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

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

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public virtual void Physics(GameTime gameTime)
        {
            Momentum.Y += 1000 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 frameMovement = Momentum * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Globals.TheTileMap.CanFall(GetRectangle(), ref frameMovement);

            Position += frameMovement;
        }
    }
}
