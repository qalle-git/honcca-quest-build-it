using HonccaBuildingGame.Classes.Extra;
using HonccaBuildingGame.Classes.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonccaBuildingGame.Classes.GameStates
{
	class SplashScreen : GameState
	{
		private Timer StartGameTimer = new Timer(12500);

		public SplashScreen()
		{
			
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();

			spriteBatch.End();
		}
	}
}
