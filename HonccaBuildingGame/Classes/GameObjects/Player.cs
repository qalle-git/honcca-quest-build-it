using HonccaBuildingGame.Classes.Extra;
using HonccaBuildingGame.Classes.Inventories;
using HonccaBuildingGame.Classes.Main;
using HonccaBuildingGame.Classes.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace HonccaBuildingGame.Classes.GameObjects
{
    class Player : Animation
    {
		private bool JumpDisabled = false;

		private readonly Timer IdleTimer = new Timer(1250);

		public Inventory ItemInventory;

        public Player(Vector2 startPosition, Texture2D startTexture) : base(startPosition, startTexture)
        {
            SetAnimationData(new Point(5, 2), new Point(0, 5), Flip.RIGHT, 120, 1);

			CurrentFrame.Y = 2;

			TextureSize.Y = 128;

            CurrentState = State.ANIMATING;

			PhysicsEnabled = true;

			ItemInventory = new Inventory();
		}

		/// <summary>
		/// Get the hitbox using a certain axis.
		/// </summary>
		/// <param name="axisToCheck">The axis to check.</param>
		/// <returns>The hitbox inside a rectangle object.</returns>
		public Rectangle GetRectangle(Axis axisToCheck)
		{
			if (axisToCheck.HasFlag(Axis.UP) || axisToCheck.HasFlag(Axis.DOWN))
			{
				return new Rectangle((int)Position.X + 8, (int)Position.Y + 10, Globals.TileSize.X - 16, Texture.Height / (TotalFrames.Y + 1) - 10);
			} 
			else if (axisToCheck.HasFlag(Axis.SIDE))
			{
				return new Rectangle((int)Position.X, (int)Position.Y + 1, Globals.TileSize.X, (Texture.Height / (TotalFrames.Y + 1)) - 12);
			}

			return new Rectangle((int)Position.X, (int)Position.Y, Globals.TileSize.X, Texture.Height / (TotalFrames.Y + 1));
		}

		public override void Update(GameTime gameTime)
		{
            Input(gameTime);
			FootstepHandler(gameTime);

			if (IdleTimer.IsFinished(gameTime))
			{
				if (CurrentFrame.X > 1)
				{
					Idle(gameTime);
				}
			}

			if (Position.X < 0)
				Position.X = 0;

			base.Update(gameTime);
		}

		#region Footsteps
		private readonly Timer FootstepTimer = new Timer(250);
		private int LastFootstepIndex = -1;

		/// <summary>
		/// This will play random footstep sounds each 250 milliseconds.
		/// </summary>
		/// <param name="gameTime">The current gameTime object.</param>
		private void FootstepHandler(GameTime gameTime)
		{
			if (JumpDisabled)
				return;

			if (Momentum.X != 0 && Momentum.Y == 0)
			{
				if (FootstepTimer.IsFinished(gameTime))
				{
					int footstepIndex = GetFootstepIndex();

					Console.WriteLine($"Playing {footstepIndex} with last {LastFootstepIndex}");

					LastFootstepIndex = footstepIndex;

					Globals.MainAudioHandler.PlaySound($"STEP_{footstepIndex + 1}", 0.25f);

					FootstepTimer.ResetTimer(gameTime);
				}
			}
		}

		/// <summary>
		/// Generate a random footstepIndex, you can not receive the last one played.
		/// </summary>
		/// <returns>A footstepindex.</returns>
		private int GetFootstepIndex()
		{
			int newFootstepIndex = Globals.RandomGenerator.Next(0, 4);

			if (newFootstepIndex == LastFootstepIndex)
				return GetFootstepIndex();

			return newFootstepIndex;
		}
		#endregion

		/// <summary>
		/// Put the player object into idle.
		/// </summary>
		/// <param name="gameTime">The current gameTime object.</param>
		private void Idle(GameTime gameTime)
		{
			if (CurrentFrame.X > 3)
				CurrentFrame.X = 0;

			CurrentFrame.Y = 0;
			FrameRange.Y = 3;
			AnimationCooldown = TimeSpan.FromMilliseconds(200);

			IdleTimer.ResetTimer(gameTime);
		}

		/// <summary>
		/// Make the player able to take inputs.
		/// </summary>
		/// <param name="gameTime">The current gameTime object.</param>
		private void Input(GameTime gameTime)
		{
			if (InputHandler.IsBeingPressed(Keys.A))
			{
				Go(gameTime, false);
			}
			else if (InputHandler.IsBeingPressed(Keys.D))
			{
				Go(gameTime, true);
			}
			else if (InputHandler.IsBeingPressed(Keys.S) && Momentum.X == 0)
			{
				Idle(gameTime);
			}
			else
			{
				Momentum.X = 0;
			}

			if (InputHandler.IsBeingPressed(Keys.W))
			{
				Jump(gameTime);
			}

			Keys[] pressedKeys = InputHandler.GetKeysCurrentlyBeingPressed();

			if (pressedKeys.Length <= 0)
			{
				Momentum.X = 0;

				Idle(gameTime);
			}
			else
			{
				if (CurrentFrame.Y > 0)
				{
					FrameRange.Y = 5;
					
					AnimationCooldown = TimeSpan.FromMilliseconds(100);
				}
			}
		}

		/// <summary>
		/// This will make the player jump.
		/// </summary>
		/// <param name="gameTime">The current gameTime object.</param>
		private void Jump(GameTime gameTime)
		{
			if (JumpDisabled)  
				return;

			JumpDisabled = true;

			Momentum.Y = -700;

			Globals.MainAudioHandler.PlaySound("JumpSound", 0.05f);
		}

		/// <summary>
		/// This will make the player go in a certain direction.
		/// </summary>
		/// <param name="gameTime">The current gameTime object.</param>
		/// <param name="right">Whether to move the character to the right or left.</param>
		private void Go(GameTime gameTime, bool right)
		{
			Momentum.X = (right ? 15000 : -15000) * (float)gameTime.ElapsedGameTime.TotalSeconds;

			CurrentFrame.Y = 2;
			CurrentState = State.ANIMATING;

			TextureDirection = right ? Flip.RIGHT : Flip.LEFT;

			IdleTimer.ResetTimer(gameTime);
		}

		/// <summary>
		/// Places a tile infront/behind/under the player depending on direction.
		/// </summary>
		/// <param name="tileIndex">The tileIndex that should be shown on the tileset.</param>
		public void PlaceTile(int tileIndex = 1)
		{
			int add = InputHandler.IsBeingPressed(Keys.S) ? 0 : TextureDirection == Flip.LEFT ? -Globals.TileSize.X : Globals.TileSize.X;

			Vector2 placePosition = new Vector2(Position.X + add, Position.Y + Globals.TileSize.Y * 2);

			Tile[] closestTiles = Globals.TheTileMap.GetClosestTile(placePosition);

			Globals.TheTileMap.Map[closestTiles[0].TileX, closestTiles[0].TileY][4] = new Tile()
			{
				TileX = closestTiles[0].TileX,
				TileY = closestTiles[0].TileY,
				TileIndex = tileIndex,
				TileType = Tile.Type.COLLISION,
				TileLayer = 4
			};

			Globals.MainAudioHandler.PlaySound("PLACE_BLOCK", 0.15f);
		}

		public override void Physics(GameTime gameTime)
		{
			Momentum.Y += 1750 * (float)gameTime.ElapsedGameTime.TotalSeconds;

			Vector2 frameMovement = Momentum * (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (Momentum.Y > 0)
			{
				if (!Globals.TheTileMap.CanFall(GetRectangle(Axis.DOWN), ref frameMovement))
				{
					Momentum.Y = 0;

					JumpDisabled = false;
				}
			} 
			else if (Momentum.Y < 0)
			{
				if (!Globals.TheTileMap.CanJump(GetRectangle(Axis.UP), ref frameMovement))
				{
					Momentum.Y = 0;
				}
			} 
			if (Momentum.X != 0)
			{
				if (!Globals.TheTileMap.CanMove(GetRectangle(Axis.SIDE), ref frameMovement))
				{
					Momentum.X = 0;
				}
			}

			Tile[] closestTiles = Globals.TheTileMap.GetClosestTile(Position);

			for (int currentTileIndex = 0; currentTileIndex < closestTiles.Length; currentTileIndex++)
			{
				Tile currentTile = closestTiles[currentTileIndex];

				if (currentTile.TileIndex == 22)
				{
					MainGame.Instance.RestartGame(gameTime);
				}
			}
			

			Position += frameMovement;
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);

			int add = InputHandler.IsBeingPressed(Keys.S) ? 0 : TextureDirection == Flip.LEFT ? -Globals.TileSize.X : Globals.TileSize.X;

			spriteBatch.Draw(Globals.MainGraphicsHandler.GetSprite("OutlineRectangle"), new Rectangle((int)Position.X + add, (int)Position.Y + Globals.TileSize.Y * 2, Globals.TileSize.X, Globals.TileSize.Y), Color.White);
		}

		/// <summary>
		/// Reset the player, this will clear everything including inventory.
		/// </summary>
		public void Reset()
		{
			ItemInventory = new Inventory();
		}
	}
}
