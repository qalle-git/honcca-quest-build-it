using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonccaBuildingGame.Classes.Main
{
	interface IGameState
	{
		public void Input(GameTime gameTime);
		public void Update(GameTime gameTime);
		public void Draw(GameTime gameTime, SpriteBatch spriteBatch);

		public bool ShouldDispose(GameTime gameTime);
	}
}
