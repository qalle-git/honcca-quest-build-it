using HonccaBuildingGame.Classes.GameObjects;
using HonccaBuildingGame.Classes.GameStates;
using HonccaBuildingGame.Classes.Main;
using HonccaBuildingGame.Classes.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace HonccaBuildingGame
{
	public class MainGame : Game
	{
		public static MainGame Instance;

		private SpriteBatch _spriteBatch;

		public MainGame()
		{
			Globals.GDManager = new GraphicsDeviceManager(this);
	
			Content.RootDirectory = "Content";

			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			Instance = this;

			Globals.GDManager.PreferredBackBufferWidth = Globals.ScreenSize.X;
			Globals.GDManager.PreferredBackBufferHeight = Globals.ScreenSize.Y;

			Globals.GDManager.ApplyChanges();

			Globals.MainCamera = new Camera();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			Globals.MainGraphicsHandler = new GraphicsHandler();

			Player thePlayer = new Player(new Vector2(Globals.ScreenSize.X / 2, 100), Globals.MainGraphicsHandler.GetSprite("BobTheBuilder"));

			Globals.MainPlayer = thePlayer;

			Globals.TheStateMachine = new StateHandler();

			StartGame();
		}

		private void StartGame()
		{
			Globals.MainPlayer.Reset();

			Globals.AllGameObjects = new List<GameObject>
			{
				Globals.MainPlayer
			};

			Globals.TheTileMap = new TileMap("LEVEL_ONE");

			Globals.TheStateMachine.AddState(new GameLoop());
			Globals.TheStateMachine.AddState(new InventoryView());
			Globals.TheStateMachine.AddState(new MapCreator());
			Globals.TheStateMachine.AddState(Globals.TheTileMap);
		}

		public void RestartGame()
		{
			Globals.TheStateMachine.Clear();

			StartGame();
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

		private void RestartHandler(GameTime gameTime)
		{
			if (InputHandler.HasKeyJustBeenPressed(Keys.R))
			{
				RestartGame();
			}
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			Globals.TheStateMachine.Draw(gameTime, _spriteBatch);

			_spriteBatch.Begin(transformMatrix: Globals.MainCamera.GetTranslationMatrix());

			for (int currentGameObjectIndex = Globals.AllGameObjects.Count - 1; currentGameObjectIndex >= 0; currentGameObjectIndex--)
			{
				Globals.AllGameObjects[currentGameObjectIndex].Draw(gameTime, _spriteBatch);
			}

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
