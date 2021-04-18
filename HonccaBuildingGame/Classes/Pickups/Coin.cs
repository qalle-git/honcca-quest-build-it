using HonccaBuildingGame.Classes.GameObjects;
using HonccaBuildingGame.Classes.Inventories;
using HonccaBuildingGame.Classes.Main;
using Microsoft.Xna.Framework;

namespace HonccaBuildingGame.Classes.Pickups
{
	class Coin : Pickup
	{
		public Coin(Vector2 _startPosition) : base(_startPosition, Globals.MainGraphicsHandler.GetSprite("Coin"))
		{
			SetAnimationData(new Point(5, 0), new Point(0, 5), Flip.LEFT, 120);

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
				Name = "COIN",
				Count = 1
			});
		}
	}
}
