using HonccaBuildingGame.Classes.GameObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonccaBuildingGame.Classes.Main
{
	class Camera
	{
		public Vector2 Position;
		private Vector2 NewPosition;

		public float Scale = 1f;

		public void Move(Vector2 newPosition)
		{
			NewPosition = newPosition;
		}

		public void Zoom(float zoomScale)
		{
			Scale *= zoomScale;
		}

		public void Focus(GameObject gameObject)
		{
			Position = new Vector2(gameObject.Position.X - (Globals.ScreenSize.X / 2), gameObject.Position.Y - (Globals.ScreenSize.Y / 2));
		}

		public void Update(GameTime gameTime)
		{
			Position.X = MathHelper.Lerp(Position.X, NewPosition.X, 0.05f);
			Position.Y = MathHelper.Lerp(Position.Y, NewPosition.Y, 0.05f);
		}

		public Matrix GetTranslationMatrix()
		{
			return Matrix.CreateTranslation(new Vector3((int)-Position.X, (int)-Position.Y, 0)) * Matrix.CreateScale(Scale);
		}
	}
}
