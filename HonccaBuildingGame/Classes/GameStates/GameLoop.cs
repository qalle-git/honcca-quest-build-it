using HonccaBuildingGame.Classes.GameObjects;
using HonccaBuildingGame.Classes.Inventories;
using HonccaBuildingGame.Classes.Main;
using HonccaBuildingGame.Classes.Pickups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonccaBuildingGame.Classes.GameStates
{
    class GameLoop : GameState
    {
		private NPC[] AllNPCs;

        public GameLoop()
		{
            Globals.MainCamera.Move(Vector2.Zero);

			Globals.MainPlayer.Position = new Vector2(Globals.TileSize.X, Globals.TileSize.Y * ((Globals.ScreenSize.Y / Globals.TileSize.Y) - 2));

			AllNPCs = new NPC[]
			{
				new NPC(new Vector2(Globals.TileSize.X * 5, Globals.TileSize.Y * 9), Globals.MainGraphicsHandler.GetSprite("NPC_ONE"), new string[] 
				{
					"Welcome to Build It!\nYour goal is to complete the puzzle course using\nthe blocks provided.",
					"Pizza for the best people."
				})
			};

			AllNPCs[0].TileSize.Y = 128;
			AllNPCs[0].SetAnimationData(new Point(3, 0), new Point(0, 3), Animation.Direction.LEFT, 190);
			AllNPCs[0].CurrentState = Animation.State.ANIMATING;

			for (int currentNPCIndex = 0; currentNPCIndex < AllNPCs.Length; currentNPCIndex++)
			{
				Globals.AllGameObjects.Add(AllNPCs[currentNPCIndex]);
			}
		}

		public override void Update(GameTime gameTime)
		{
			CameraHandler(gameTime);

			NPCHandler(gameTime);
			PickupHandler(gameTime);

			base.Update(gameTime);
		}

		private void NPCHandler(GameTime gameTime)
		{
			for (int currentNPCIndex = 0; currentNPCIndex < AllNPCs.Length; currentNPCIndex++)
			{
				NPC currentNPC = AllNPCs[currentNPCIndex];

				float distance = Vector2.Distance(Globals.MainPlayer.Position, currentNPC.Position);

				if (distance <= 40)
				{
					if (InputHandler.HasKeyJustBeenPressed(Keys.F))
					{
						currentNPC.NPCDialogue.StartDialogue(gameTime);
					}
				}
			}
		}

		private void CameraHandler(GameTime gameTime)
		{
			for (int currentCheckIndex = 0; currentCheckIndex < Globals.ScreenSize.X / Globals.TileSize.X; currentCheckIndex++)
			{
				int maxInterval = Globals.ScreenSize.X * (currentCheckIndex + 1);
				int minInterval = Globals.ScreenSize.X * currentCheckIndex;

				if (Globals.MainPlayer.Position.X > minInterval && Globals.MainPlayer.Position.X < maxInterval && Globals.MainCamera.Position.X != minInterval)
				{
					Globals.MainCamera.Move(new Vector2(minInterval, Globals.MainCamera.Position.Y));
				}
			}
		}

		private void PickupHandler(GameTime gameTime)
		{
			for (int currentGameObjectIndex = Globals.AllGameObjects.Count - 1; currentGameObjectIndex >= 0; currentGameObjectIndex--)
			{
				GameObject gameObject = Globals.AllGameObjects[currentGameObjectIndex];

				if (gameObject.GetType().BaseType == typeof(Pickup))
				{
					float distance = Vector2.Distance(Globals.MainPlayer.Position, gameObject.Position);

					if (distance <= Globals.TileSize.X + 8)
					{
						Globals.AllGameObjects.RemoveAt(currentGameObjectIndex);

						OnPickup(gameTime, gameObject);
					}
				}
			}
		}

		private void OnPickup(GameTime gameTime, GameObject pickupObject)
		{
			Type pickupType = pickupObject.GetType();

			if (pickupType == typeof(Coin))
			{
				Globals.MainPlayer.ItemInventory.AddItem(new Item()
				{
					Name = "COIN",
					Count = 1
				});
			}
			else if (pickupType == typeof(Block))
			{
				Globals.MainPlayer.ItemInventory.AddItem(new Item()
				{
					Name = Globals.MainGraphicsHandler.GetSpriteNameFromTexture2D(pickupObject.Texture),
					Count = 1
				});
			}
		}
	}
}
