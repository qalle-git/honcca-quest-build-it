using HonccaBuildingGame.Classes.Extra;
using HonccaBuildingGame.Classes.GameObjects;
using HonccaBuildingGame.Classes.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HonccaBuildingGame.Classes.GameStates
{
    class GameLoop : GameState
    {
		private readonly NPC[] AllNPCs;

        public GameLoop()
		{
            Globals.MainCamera.Move(Vector2.Zero);

			Globals.MainPlayer.Position = new Vector2(Globals.TileSize.X, Globals.TileSize.Y * ((Globals.ScreenSize.Y / Globals.TileSize.Y) - 2));

			// This is static peds that you can conversate with.
			AllNPCs = new NPC[]
			{
				new NPC(new Vector2(Globals.TileSize.X * 5, Globals.TileSize.Y * 9), Globals.MainGraphicsHandler.GetSprite("NPC_ONE"), new string[] 
				{
					"Welcome to Build It!\nTry placing the block I gave you\ninside the white rectangle using E."
				}),
				new NPC(new Vector2(Globals.TileSize.X * 24, Globals.TileSize.Y * 7), Globals.MainGraphicsHandler.GetSprite("NPC_ONE"), new string[]
				{
					"You can use S while jumping to place a block underneath you.",
					"Try it out."
				}),
				new NPC(new Vector2(Globals.TileSize.X * 50, Globals.TileSize.Y * 8), Globals.MainGraphicsHandler.GetSprite("NPC_ONE"), new string[]
				{
					"Hey, how did you get in here?",
					"Try building up my balcony."
				}),
				new NPC(new Vector2(Globals.TileSize.X * 117, Globals.TileSize.Y * 9), Globals.MainGraphicsHandler.GetSprite("NPC_ONE"), new string[]
				{
					"Good Job!\nThanks for running in my crazy city."
				})
			};

			for (int currentNPCIndex = 0; currentNPCIndex < AllNPCs.Length; currentNPCIndex++)
			{
				AllNPCs[currentNPCIndex].TextureSize.Y = 128;
				AllNPCs[currentNPCIndex].SetAnimationData(new Point(3, 0), new Point(0, 3), Animation.Flip.LEFT, 190);
				AllNPCs[currentNPCIndex].CurrentState = Animation.State.ANIMATING;
			}

			for (int currentNPCIndex = 0; currentNPCIndex < AllNPCs.Length; currentNPCIndex++)
			{
				Globals.AllGameObjects.Add(AllNPCs[currentNPCIndex]);
			}
		}

		public override void Update(GameTime gameTime)
		{
			CameraHandler(gameTime);

			PickupHandler(gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This method makes the npcs interactable.
		/// </summary>
		/// <param name="gameTime">The current gameTime object.</param>
		/// <param name="spriteBatch">The current spriteBatch object.</param>
		private void NPCHandler(GameTime gameTime, SpriteBatch spriteBatch)
		{
			for (int currentNPCIndex = 0; currentNPCIndex < AllNPCs.Length; currentNPCIndex++)
			{
				NPC currentNPC = AllNPCs[currentNPCIndex];

				float distance = Vector2.Distance(Globals.MainPlayer.Position, currentNPC.Position);

				if (distance <= Globals.TileSize.X * 1.5f)
				{
					if (InputHandler.HasKeyJustBeenPressed(Keys.F))
					{
						currentNPC.NPCDialogue.StartDialogue(gameTime);
					}

					if (currentNPC.NPCDialogue.CurrentDialogue < currentNPC.NPCDialogue.DialogueCount - 1)
					{
						Texture2D fButtonTexture = Globals.MainGraphicsHandler.GetSprite("BUTTON_F");

						spriteBatch.Draw(fButtonTexture, new Rectangle((int)Globals.MainPlayer.Position.X, (int)Globals.MainPlayer.Position.Y - Globals.TileSize.Y, Globals.TileSize.X, Globals.TileSize.Y), Color.White);
					}
				}
				else
				{
					currentNPC.NPCDialogue.CurrentDialogue = -1;
				}
			}
		}

		/// <summary>
		/// This method will move the camera to the correct scene, uses the players location.
		/// </summary>
		/// <param name="gameTime">The current gameTime object.</param>
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

		/// <summary>
		/// This method makes each pickup "pickupable".
		/// </summary>
		/// <param name="gameTime">The current gameTime object.</param>
		private void PickupHandler(GameTime gameTime)
		{
			for (int currentGameObjectIndex = Globals.AllGameObjects.Count - 1; currentGameObjectIndex >= 0; currentGameObjectIndex--)
			{
				if (Globals.AllGameObjects.Count < currentGameObjectIndex)
					break;

				GameObject gameObject = Globals.AllGameObjects[currentGameObjectIndex];

				if (gameObject.GetType().BaseType == typeof(Pickup))
				{
					Pickup pickupObject = (Pickup)gameObject;

					float distance = Vector2.Distance(Globals.MainPlayer.Position, pickupObject.Position);

					if (distance <= Pickup.PickupDistance)
					{
						Globals.AllGameObjects.RemoveAt(currentGameObjectIndex);

						pickupObject.OnPickup(gameTime);
					}
				}
			}
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);

			spriteBatch.Begin(transformMatrix: Globals.MainCamera.GetTranslationMatrix());

			NPCHandler(gameTime, spriteBatch);

			DrawInstructions(gameTime, spriteBatch);

			spriteBatch.End();
		}

		/// <summary>
		/// This will draw the instructions in the beginning of the game.
		/// </summary>
		/// <param name="gameTime">The current gameTime object.</param>
		/// <param name="spriteBatch">The current spriteBatch object.</param>
		private void DrawInstructions(GameTime gameTime, SpriteBatch spriteBatch)
		{
			Texture2D rButtonTexture = Globals.MainGraphicsHandler.GetSprite("BUTTON_R");

			spriteBatch.Draw(rButtonTexture, new Rectangle(Globals.TileSize.X * 4, Globals.TileSize.Y * 3, Globals.TileSize.X, Globals.TileSize.Y), Color.White);
			spriteBatch.DrawString(Dialogue.DialogueFont, "= Restart", new Vector2(Globals.TileSize.X * 5, Globals.TileSize.Y * 3 + Dialogue.DialogueFont.MeasureString("= Restart").Y), Color.White);

			Texture2D eButtonTexture = Globals.MainGraphicsHandler.GetSprite("BUTTON_E");

			spriteBatch.Draw(eButtonTexture, new Rectangle(Globals.TileSize.X * 4, Globals.TileSize.Y * 4, Globals.TileSize.X, Globals.TileSize.Y), Color.White);
			spriteBatch.DrawString(Dialogue.DialogueFont, "= Place", new Vector2(Globals.TileSize.X * 5, Globals.TileSize.Y * 4 + Dialogue.DialogueFont.MeasureString("= Place").Y), Color.White);

			Texture2D fButtonTexture = Globals.MainGraphicsHandler.GetSprite("BUTTON_F");

			spriteBatch.Draw(fButtonTexture, new Rectangle(Globals.TileSize.X * 4, Globals.TileSize.Y * 5, Globals.TileSize.X, Globals.TileSize.Y), Color.White);
			spriteBatch.DrawString(Dialogue.DialogueFont, "= Interact", new Vector2(Globals.TileSize.X * 5, Globals.TileSize.Y * 5 + Dialogue.DialogueFont.MeasureString("= Interact").Y), Color.White);
		}
	}
}
