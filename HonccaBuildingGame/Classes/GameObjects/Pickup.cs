using HonccaBuildingGame.Classes.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HonccaBuildingGame.Classes.GameObjects
{
	abstract class Pickup : Animation
	{
		public static int PickupDistance = (int)(Globals.TileSize.X * 1.2f);

		public Pickup(Vector2 _startPosition, Texture2D _texture) : base(_startPosition, _texture)
		{

		}

		/// <summary>
		/// This will be triggered when the pickup is picked up.
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void OnPickup(GameTime gameTime)
		{
			Globals.MainAudioHandler.PlaySound("PICKUP_ITEM", 0.1f);
		}
	}
}
