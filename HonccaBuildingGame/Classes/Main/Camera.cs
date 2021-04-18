using Microsoft.Xna.Framework;

namespace HonccaBuildingGame.Classes.Main
{
	class Camera
	{
		public Vector2 Position;
		private Vector2 NewPosition;

		public float Scale = 1f;

		/// <summary>
		/// Move the camera (animation)
		/// </summary>
		/// <param name="newPosition">The end position the camera should end on.</param>
		public void Move(Vector2 newPosition)
		{
			NewPosition = newPosition;
		}

		public void ForceMove(Vector2 newPosition)
		{
			Position = newPosition;
			NewPosition = newPosition;
		}

		/// <summary>
		/// Zoom the camera.
		/// </summary>
		/// <param name="zoomScale">The amount you want to zoom.</param>
		public void Zoom(float zoomScale)
		{
			Scale *= zoomScale;
		}

		/// <summary>
		/// Updates to the correct position using lerp as the animation.
		/// </summary>
		/// <param name="gameTime">The current gameTime object.</param>
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
