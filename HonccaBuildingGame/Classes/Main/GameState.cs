using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonccaBuildingGame.Classes.Main
{
	abstract class GameState : IGameState
	{
		// If this is set to false, nothing will draw on this gameState.
		public bool Visible = true;

		public virtual void Input(GameTime gameTime)
		{
		}

		public virtual void Update(GameTime gameTime)
		{
		}

		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
		}

		/// <summary>
		/// If you return true in this method it will make the gameState dissapear.
		/// </summary>
		/// <param name="gameTime">The current gameTime object.</param>
		/// <returns>It returns a bool whether to shut down the gameObject or not.</returns>
		public virtual bool ShouldDispose(GameTime gameTime)
		{
			return false;
		}
	}
}
