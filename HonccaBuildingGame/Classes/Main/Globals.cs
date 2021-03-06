using HonccaBuildingGame.Classes.GameObjects;
using HonccaBuildingGame.Classes.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace HonccaBuildingGame.Classes.Main
{
	static class Globals
	{
		public static Point ScreenSize = new Point(1280, 720);
		public static Point GameSize = new Point(150, 50);

		public static Point TileSize = new Point(64, 64);

		public static GraphicsDeviceManager GDManager;

		public static Random RandomGenerator;

		public static GraphicsHandler MainGraphicsHandler;
		public static AudioHandler MainAudioHandler;

		public static Player MainPlayer;
		public static List<GameObject> AllGameObjects = new List<GameObject>();

		public static Camera MainCamera;

		public static StateHandler TheStateMachine;
		public static TileMap TheTileMap;
	}
}
