using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonccaBuildingGame.Classes.Main
{
	abstract class GameState : IGameState
	{
		public bool Visible = true;

		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
		}

		public virtual void Input(GameTime gameTime)
		{
		}

		public virtual void Update(GameTime gameTime)
		{
		}

		public virtual bool ShouldDispose()
		{
			return false;
		}
	}
}
