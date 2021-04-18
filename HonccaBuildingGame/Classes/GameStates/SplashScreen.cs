using HonccaBuildingGame.Classes.Extra;
using HonccaBuildingGame.Classes.GameObjects;
using HonccaBuildingGame.Classes.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace HonccaBuildingGame.Classes.GameStates
{
	class SplashScreen : GameState
	{
		public static SpriteFont TitleFont;
		public static SpriteFont PressAnyKeyFont;

		private bool Reverse = false;

		private readonly Timer FadeTimer = new Timer(1500);
		private Timer StartGameTimer;

		private readonly Animation PlayerShowcase;

		public SplashScreen()
		{
			TitleFont = MainGame.Instance.Content.Load<SpriteFont>("Fonts/titleFont");
			PressAnyKeyFont = MainGame.Instance.Content.Load<SpriteFont>("Fonts/pressAnyKeyFont");

			PlayerShowcase = new Animation(new Vector2(Globals.ScreenSize.X / 2 - Globals.TileSize.X / 2, Globals.TileSize.Y * 4), Globals.MainGraphicsHandler.GetSprite("BobTheBuilder"));
			PlayerShowcase.SetAnimationData(new Point(5, 2), new Point(0, 4), Animation.Flip.RIGHT, 120, 1);

			PlayerShowcase.CurrentFrame.Y = 0;
			PlayerShowcase.CurrentState = Animation.State.ANIMATING;

			PlayerShowcase.TextureSize.Y = 128;

			MediaPlayer.Play(MainGame.MainMenuSong);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (FadeTimer.IsFinished(gameTime))
			{
				FadeTimer.ResetTimer(gameTime);

				Reverse = !Reverse;
			}

			if (InputHandler.GetKeysCurrentlyBeingPressed().Length > 0)
			{
				StartTimer(gameTime);
			}

			MouseState mouseState = Mouse.GetState();

			if (mouseState.LeftButton == ButtonState.Pressed)
			{
				StartTimer(gameTime);
			}
			else if (mouseState.RightButton == ButtonState.Pressed)
			{
				StartTimer(gameTime);
			}

			if (StartGameTimer != null)
			{
				if (StartGameTimer.IsFinished(gameTime))
				{
					MediaPlayer.Play(MainGame.BackgroundSong);
					MediaPlayer.Volume = 0.003f;

					MainGame.Instance.RestartGame(gameTime);
				}
			}

			PlayerShowcase.Update(gameTime);
		}

		/// <summary>
		/// Start the timer that will count down to start the real game.
		/// </summary>
		/// <param name="gameTime">The current gametime object.</param>
		private void StartTimer(GameTime gameTime)
		{
			if (StartGameTimer != null)
				return;

			StartGameTimer = new Timer(250);

			StartGameTimer.ResetTimer(gameTime);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();

			float fadePercent = FadeTimer.GetTimerInPercent(gameTime);

			float currentPercent = (Reverse ? (1 - fadePercent / 100) : fadePercent / 100);

			const string keyString = "Press ANY key to start";

			spriteBatch.DrawString(PressAnyKeyFont, keyString, new Vector2(Globals.ScreenSize.X / 2 - PressAnyKeyFont.MeasureString(keyString).X / 2, Globals.ScreenSize.Y / 6 * 5), Color.Black * currentPercent);

			const string gameNameString = "Build It!";

			spriteBatch.DrawString(TitleFont, gameNameString, new Vector2(Globals.ScreenSize.X / 2 - TitleFont.MeasureString(gameNameString).X / 2, Globals.TileSize.X * 2 - TitleFont.MeasureString(gameNameString).Y / 2), Color.Black);

			PlayerShowcase.Draw(gameTime, spriteBatch);

			spriteBatch.End();
		}

		public override bool ShouldDispose(GameTime gameTime)
		{
			return StartGameTimer != null && StartGameTimer.IsFinished(gameTime);
		}
	}
}
