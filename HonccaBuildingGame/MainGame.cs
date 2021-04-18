using HonccaBuildingGame.Classes.GameObjects;
using HonccaBuildingGame.Classes.GameStates;
using HonccaBuildingGame.Classes.Main;
using HonccaBuildingGame.Classes.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace HonccaBuildingGame
{
	public class MainGame : Game
	{
		public static MainGame Instance;

		private SpriteBatch TheSpriteBatch;

		public static Song MainMenuSong;
		public static Song BackgroundSong;

		public static TimeSpan GameStarted;

		public MainGame()
		{
			Globals.GDManager = new GraphicsDeviceManager(this);
	
			Content.RootDirectory = "Content";

			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			Instance = this;

			MediaPlayer.IsRepeating = true;
			MediaPlayer.Volume = 0.1f;

			Globals.GDManager.PreferredBackBufferWidth = Globals.ScreenSize.X;
			Globals.GDManager.PreferredBackBufferHeight = Globals.ScreenSize.Y;

			Globals.GDManager.ApplyChanges();

			Globals.MainCamera = new Camera();

			Globals.RandomGenerator = new Random();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			TheSpriteBatch = new SpriteBatch(GraphicsDevice);

			MainMenuSong = Content.Load<Song>("Audio/mainMenuAudio");
			BackgroundSong = Content.Load<Song>("Audio/mainBackgroundTheme");

			Globals.MainGraphicsHandler = new GraphicsHandler();
			Globals.MainAudioHandler = new AudioHandler();

			Player thePlayer = new Player(new Vector2(Globals.ScreenSize.X / 2, 100), Globals.MainGraphicsHandler.GetSprite("BobTheBuilder"));

			Globals.MainPlayer = thePlayer;

			Globals.TheStateMachine = new StateHandler();

			Globals.TheStateMachine.AddState(new SplashScreen());

			Globals.TheTileMap = new TileMap("SPLASH_SCREEN");
			Globals.TheStateMachine.AddState(Globals.TheTileMap);

			//StartGame();
		}

		/// <summary>
		/// This will start the game.
		/// </summary>
		/// <param name="gameTime">The current gameTime object.</param>
		private void StartGame(GameTime gameTime)
		{
			Globals.MainPlayer.Reset();

			Globals.AllGameObjects = new List<GameObject>
			{
				Globals.MainPlayer
			};

			Globals.TheStateMachine.AddState(new GameLoop());
			Globals.TheStateMachine.AddState(new InventoryView());
			//Globals.TheStateMachine.AddState(new MapCreator());

			Globals.TheTileMap = new TileMap("LEVEL_ONE");

			Globals.TheStateMachine.AddState(Globals.TheTileMap);

			GameStarted = gameTime.TotalGameTime;
		}

		/// <summary>
		/// This will restart the whole game.
		/// </summary>
		/// <param name="gameTime">The current gameTime object.</param>
		public void RestartGame(GameTime gameTime)
		{
			Globals.TheStateMachine.Clear();

			MediaPlayer.Play(MainGame.BackgroundSong);
			MediaPlayer.Volume = 0.003f;

			StartGame(gameTime);
		}

		protected override void Update(GameTime gameTime)
		{
			InputHandler.RefreshKeyboardState();

			if (InputHandler.HasKeyJustBeenPressed(Keys.Escape))
				Exit();

			Globals.TheStateMachine.Input(gameTime);
			Globals.TheStateMachine.Update(gameTime);

			for (int currentGameObjectIndex = 0; currentGameObjectIndex < Globals.AllGameObjects.Count; currentGameObjectIndex++)
			{
				Globals.AllGameObjects[currentGameObjectIndex].Update(gameTime);
			}

			Globals.MainCamera.Update(gameTime);

			RestartHandler(gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This will check if the user is currently pressing R and then restart the game.
		/// </summary>
		/// <param name="gameTime">The current gameTime object.</param>
		private void RestartHandler(GameTime gameTime)
		{
			if (InputHandler.HasKeyJustBeenPressed(Keys.R))
			{
				RestartGame(gameTime);
			}
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			Globals.TheStateMachine.Draw(gameTime, TheSpriteBatch);

			TheSpriteBatch.Begin(transformMatrix: Globals.MainCamera.GetTranslationMatrix());

			for (int currentGameObjectIndex = Globals.AllGameObjects.Count - 1; currentGameObjectIndex >= 0; currentGameObjectIndex--)
			{
				Globals.AllGameObjects[currentGameObjectIndex].Draw(gameTime, TheSpriteBatch);
			}

			TheSpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
