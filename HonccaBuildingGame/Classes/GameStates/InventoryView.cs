using HonccaBuildingGame.Classes.Inventories;
using HonccaBuildingGame.Classes.Main;
using HonccaBuildingGame.Classes.Pickups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static HonccaBuildingGame.Classes.GameObjects.Animation;

namespace HonccaBuildingGame.Classes.GameStates
{
	class InventoryView : GameState
	{
		private readonly SpriteFont CountFont;

		private int InventorySlot = 0;
		// Which keys control slot 1,2,3,4.
		private readonly Keys[] InventoryKeys = new Keys[]
		{
			Keys.D1,
			Keys.D2,
			Keys.D3,
			Keys.D4
		};

		// This is the size of the inventory item slot image.
		private const int IconSize = 32;

		public InventoryView() => CountFont = MainGame.Instance.Content.Load<SpriteFont>("Fonts/pixelFont");

		public override void Input(GameTime gameTime)
		{
			base.Input(gameTime);


			for (int currentKeyIndex = 0; currentKeyIndex < InventoryKeys.Length; currentKeyIndex++)
			{
				if (InputHandler.HasKeyJustBeenPressed(InventoryKeys[currentKeyIndex]))
				{
					InventorySlot = currentKeyIndex;
				}
			}

			if (InputHandler.HasKeyJustBeenPressed(Keys.Q))
			{
				DropHoldingItem(gameTime);
			}
			if (InputHandler.HasKeyJustBeenPressed(Keys.E))
			{
				PlaceHoldingItem(gameTime);
			}
		}

		/// <summary>
		/// Place the item the player is currently holding.
		/// </summary>
		/// <param name="gameTime">Current GameTime object.</param>
		private void PlaceHoldingItem(GameTime gameTime)
		{
			Inventory playerInventory = Globals.MainPlayer.ItemInventory;

			Item holdingItem = playerInventory.GetItemOnSlot(InventorySlot);

			// Isn't holding in any item.
			if (holdingItem.Name == null)
				return;

			Dictionary<string, int> itemToTileIndex = new Dictionary<string, int>()
			{
				{
					"DIRT_BLOCK",
					72
				}
			};

			if (itemToTileIndex.ContainsKey(holdingItem.Name))
			{
				Globals.MainPlayer.PlaceTile(itemToTileIndex[holdingItem.Name]);

				playerInventory.RemoveItem(new Item()
				{
					Name = holdingItem.Name,
					Count = 1
				});
			}
		}

		/// <summary>
		/// Drop the item the player is currently holding.
		/// </summary>
		/// <param name="gameTime">Current GameTime object.</param>
		private void DropHoldingItem(GameTime gameTime)
		{
			Inventory playerInventory = Globals.MainPlayer.ItemInventory;

			Item holdingItem = playerInventory.GetItemOnSlot(InventorySlot);

			bool facingRight = Globals.MainPlayer.TextureDirection == Flip.RIGHT;
			bool isIdle = InputHandler.IsBeingPressed(Keys.S);

			int pixelsToAdd = isIdle ? 0 : facingRight ? (Globals.TileSize.X + 16) : -(Globals.TileSize.X + 16);

			if (holdingItem.Count > 0)
			{
				if (holdingItem.Name == "COIN")
				{
					Coin newCoin = new Coin(new Vector2(Globals.MainPlayer.Position.X + pixelsToAdd, Globals.MainPlayer.Position.Y - (isIdle ? Globals.TileSize.Y * 1.5f : 0)));

					if (isIdle)
					{
						newCoin.Momentum.Y = -250;
					}
					else
					{
						newCoin.Momentum.X = facingRight ? 300 : -300;
					}

					Globals.AllGameObjects.Add(newCoin);
				}
				else if (holdingItem.Name == "DIRT_BLOCK")
				{
					Block newBlock = new Block(new Vector2(Globals.MainPlayer.Position.X + pixelsToAdd, Globals.MainPlayer.Position.Y - (isIdle ? Globals.TileSize.Y * 1.5f : 0)), holdingItem.Name);

					if (isIdle)
					{
						newBlock.Momentum.Y = -250;
					}
					else
					{
						newBlock.Momentum.X = facingRight ? 300 : -300;
					}

					Globals.AllGameObjects.Add(newBlock);
				}

				playerInventory.RemoveItem(new Item()
				{
					Name = holdingItem.Name,
					Count = 1
				});
			}
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (!Visible)
				return;

			spriteBatch.Begin();

			Inventory playerInventory = Globals.MainPlayer.ItemInventory;

			for (int currentItemSlot = 0; currentItemSlot < playerInventory.Items.Length; currentItemSlot++)
			{
				Item currentItem = playerInventory.Items[currentItemSlot];

				Texture2D inventorySlotTexture = InventorySlot == currentItemSlot ? Globals.MainGraphicsHandler.GetSprite("SelectedInventorySlot") : Globals.MainGraphicsHandler.GetSprite("InventorySlot");

				spriteBatch.Draw(inventorySlotTexture, new Rectangle(currentItemSlot * Globals.TileSize.X, 0, Globals.TileSize.X, Globals.TileSize.Y), Color.White);
				
				if (currentItem.Count > 0)
				{
					spriteBatch.Draw(Globals.MainGraphicsHandler.GetSprite(currentItem.Name), new Rectangle(currentItemSlot * Globals.TileSize.X + IconSize / 2, 0 + IconSize / 2, Globals.TileSize.X - IconSize, Globals.TileSize.Y - IconSize), Color.White);

					spriteBatch.DrawString(CountFont, currentItem.Count.ToString(), new Vector2(currentItemSlot * Globals.TileSize.X + 9, 9), Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
				}
			}

			spriteBatch.End();
		}
	}
}
