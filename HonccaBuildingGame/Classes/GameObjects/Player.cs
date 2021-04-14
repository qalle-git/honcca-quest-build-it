using HonccaBuildingGame.Classes.Extra;
using HonccaBuildingGame.Classes.Inventories;
using HonccaBuildingGame.Classes.Main;
using HonccaBuildingGame.Classes.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonccaBuildingGame.Classes.GameObjects
{
    class Player : Animation
    {
		private bool JumpDisabled = false;

		private readonly Timer IdleTimer = new Timer(1250);

		public Inventory ItemInventory;

        public Player(Vector2 startPosition, Texture2D startTexture) : base(startPosition, startTexture)
        {
            SetAnimationData(new Point(5, 2), new Point(0, 5), Direction.RIGHT, 120, 1);

			CurrentFrame.Y = 2;

			TileSize.Y = 128;

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

			if (IdleTimer.IsFinished(gameTime))
			{
				if (CurrentFrame.X > 1)
				{
					Idle(gameTime);
				}

			}

			base.Update(gameTime);
		}

		private void Idle(GameTime gameTime)
		{
			if (CurrentFrame.X > 3)
				CurrentFrame.X = 0;

			CurrentFrame.Y = 0;
			FrameRange.Y = 3;
			AnimationCooldown = TimeSpan.FromMilliseconds(200);

			IdleTimer.ResetTimer(gameTime);
		}

		private void Input(GameTime gameTime)
		{
			KeyboardState keyboardState = Keyboard.GetState();

			if (keyboardState.IsKeyDown(Keys.A))
			{
				Momentum.X = -15000 * (float)gameTime.ElapsedGameTime.TotalSeconds;

				CurrentFrame.Y = 2;
				CurrentState = State.ANIMATING;
				CurrentDirection = Direction.LEFT;

				IdleTimer.ResetTimer(gameTime);
			}
			else if (keyboardState.IsKeyDown(Keys.D))
			{
				Momentum.X = 15000 * (float)gameTime.ElapsedGameTime.TotalSeconds;

				CurrentFrame.Y = 2;
				CurrentState = State.ANIMATING;
				CurrentDirection = Direction.RIGHT;

				IdleTimer.ResetTimer(gameTime);
			}
			else if (keyboardState.IsKeyDown(Keys.S) && Momentum.X == 0)
			{
				Idle(gameTime);
			}
			else
			{
				Momentum.X = 0;
			}

			if (keyboardState.IsKeyDown(Keys.W))
			{
				if (!JumpDisabled)
				{
					JumpDisabled = true;

					Momentum.Y = -700;
				}
			}

			Keys[] pressedKeys = keyboardState.GetPressedKeys();

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

		public void PlaceTile(int tileIndex = 1)
		{
			int add = InputHandler.IsBeingPressed(Keys.S) ? 0 : CurrentDirection == Direction.LEFT ? -Globals.TileSize.X : Globals.TileSize.X;

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
			} else if (Momentum.Y < 0)
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
					MainGame.Instance.RestartGame();
				}
			}
			

			Position += frameMovement;
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);

			int add = InputHandler.IsBeingPressed(Keys.S) ? 0 : CurrentDirection == Direction.LEFT ? -Globals.TileSize.X : Globals.TileSize.X;

			spriteBatch.Draw(Globals.MainGraphicsHandler.GetSprite("OutlineRectangle"), new Rectangle((int)Position.X + add, (int)Position.Y + Globals.TileSize.Y * 2, Globals.TileSize.X, Globals.TileSize.Y), Color.White);
		}

		public void Reset()
		{
			ItemInventory = new Inventory();
		}
	}
}
