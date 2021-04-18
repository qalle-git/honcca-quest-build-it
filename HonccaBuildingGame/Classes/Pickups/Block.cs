using HonccaBuildingGame.Classes.GameObjects;
using HonccaBuildingGame.Classes.Inventories;
using HonccaBuildingGame.Classes.Main;
using Microsoft.Xna.Framework;

namespace HonccaBuildingGame.Classes.Pickups
{
    class Block : Pickup
    {
		public Block(Vector2 _startPosition, string _blockName) : base(_startPosition, Globals.MainGraphicsHandler.GetSprite(_blockName))
		{
			SetAnimationData(Point.Zero, Point.Zero, Flip.LEFT, 120);

			TextureSize.Y = 64;

			CurrentState = State.ANIMATING;

			PhysicsEnabled = true;
		}

		public override void Physics(GameTime gameTime)
		{
			Momentum.Y += 750 * (float)gameTime.ElapsedGameTime.TotalSeconds;

			Vector2 frameMovement = Momentum * (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (Momentum.Y > 0)
			{
				if (!Globals.TheTileMap.CanFall(GetRectangle(), ref frameMovement))
				{
					Momentum.Y = 0;
				}
			}
			if (Momentum.X > 0)
			{
				Momentum.X -= 750 * (float)gameTime.ElapsedGameTime.TotalSeconds;

				if (MathHelper.Distance(Momentum.X, 0) < 1)
				{
					Momentum.X = 0;
				}
			}
			else if (Momentum.X < 0)
			{
				Momentum.X += 750 * (float)gameTime.ElapsedGameTime.TotalSeconds;

				if (MathHelper.Distance(Momentum.X, 0) < 1)
				{
					Momentum.X = 0;
				}
			}

			Position += frameMovement;
		}

		public override void OnPickup(GameTime gameTime)
		{
			base.OnPickup(gameTime);

			Globals.MainPlayer.ItemInventory.AddItem(new Item()
			{
				Name = Globals.MainGraphicsHandler.GetSpriteNameFromTexture2D(Texture),
				Count = 1
			});
		}
	}
}
