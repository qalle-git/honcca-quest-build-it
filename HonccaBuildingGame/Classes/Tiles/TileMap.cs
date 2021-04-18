using HonccaBuildingGame.Classes.Main;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using HonccaBuildingGame.Classes.Extra;
using HonccaBuildingGame.Classes.Pickups;

namespace HonccaBuildingGame.Classes.Tiles
{
    class TileMap : GameState
    {
        public Tile[,][] Map;

        /// <summary>
        /// Get the tile map dimensions.
        /// </summary>
        public Point SheetDimensions
        {
            get
            {
                Texture2D tileSet = Globals.MainGraphicsHandler.GetSprite("MainTileSet");

                return new Point(tileSet.Width / Globals.TileSize.X, tileSet.Height / Globals.TileSize.Y);
            }
        }

        public TileMap(string fileName)
        {
            LoadMapFromFile(fileName);

            SpawnPickups();
        }

        /// <summary>
        /// Get a list of the closest tiles.
        /// </summary>
        /// <param name="placePosition">The position you want to check from.</param>
        /// <returns>A array with all the closest tiles.</returns>
        public Tile[] GetClosestTile(Vector2 placePosition)
        {
            Point lastTilePosition = new Point(-1, -1);
            float lastDistance = Globals.TileSize.X;

            for (int currentX = 0; currentX < Map.GetLength(0); currentX++)
            {
                for (int currentY = 0; currentY < Map.GetLength(1); currentY++)
                {
                    float currentDistance = Vector2.Distance(placePosition, new Vector2(currentX * Globals.TileSize.X, currentY * Globals.TileSize.Y));

                    if (currentDistance < lastDistance)
                    {
                        lastDistance = currentDistance;
                        lastTilePosition = new Point(currentX, currentY);
                    }
                }
            }

            if (lastTilePosition.X >= 0)
            {
                return Map[lastTilePosition.X, lastTilePosition.Y];
            } 

            return new Tile[]
            {
                new Tile()
                {
                    TileIndex = 0
                }
            };
        }

        /// <summary>
        /// This determines whether the gameObject can move to the sides or not.
        /// </summary>
        /// <param name="hitbox">The hitbox of the gameObject.</param>
        /// <param name="frameMovement">How far the gameObject will move in this frame.</param>
        /// <returns>Whether the gameObject can move or not.</returns>
        public bool CanMove(Rectangle hitbox, ref Vector2 frameMovement)
        {
            int currentX = (int)Math.Floor((hitbox.X + frameMovement.X) / Globals.TileSize.X);

            if (frameMovement.X > 0)
            {
                currentX = (int)Math.Floor((hitbox.X + hitbox.Width + frameMovement.X) / Globals.TileSize.X);
            }

            int minimumY = (int)Math.Floor((hitbox.Y + frameMovement.Y + 1) / Globals.TileSize.Y);
            int maximumY = (int)Math.Floor((hitbox.Y + hitbox.Height + frameMovement.Y) / Globals.TileSize.Y);

            for (int currentY = minimumY; currentY <= maximumY; currentY++)
            {
                if (GetCollisionAtPosition(currentX, currentY) == Tile.Type.COLLISION)
                {
                    if (frameMovement.X > 0)
                    {
                        frameMovement.X = Math.Max(0, currentX * Globals.TileSize.X - hitbox.X - hitbox.Width);
                    } 
                    else
                    {
                        frameMovement.X = Math.Min(0, (currentX + 1) * Globals.TileSize.X - hitbox.X);
                    }

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// This determines whether the gameObject can fall or not.
        /// </summary>
        /// <param name="hitbox">The hitbox of the gameObject.</param>
        /// <param name="frameMovement">How far the gameObject will move in this frame.</param>
        /// <returns>Whether the gameObject can fall or not.</returns>
        public bool CanFall(Rectangle hitbox, ref Vector2 frameMovement)
        {
            int minimumX = (int)Math.Floor((hitbox.X + frameMovement.X) / Globals.TileSize.X);
            int maximumX = (int)Math.Floor((hitbox.X + hitbox.Width + frameMovement.X) / Globals.TileSize.X);

            int currentY = (int)Math.Floor((hitbox.Y + hitbox.Height + frameMovement.Y) / Globals.TileSize.Y);

            for (int currentX = minimumX; currentX <= maximumX; currentX++)
            {
                if (GetCollisionAtPosition(currentX, currentY) == Tile.Type.COLLISION)
                {
                    frameMovement.Y = currentY * Globals.TileSize.Y - hitbox.Y - hitbox.Height;

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// This determines whether the gameObject can jump or not.
        /// </summary>
        /// <param name="hitbox">The hitbox of the gameObject.</param>
        /// <param name="frameMovement">How far the gameObject will move in this frame.</param>
        /// <returns>Whether the gameObject can jump or not.</returns>
        public bool CanJump(Rectangle hitbox, ref Vector2 frameMovement)
        {
            int minimumX = (int)Math.Floor((hitbox.X + frameMovement.X) / Globals.TileSize.X);
            int maximumX = (int)Math.Floor((hitbox.X + hitbox.Width + frameMovement.X) / Globals.TileSize.X);

            int currentY = (int)Math.Floor((hitbox.Y + hitbox.Height + frameMovement.Y) / Globals.TileSize.Y) - 2;

            for (int currentX = minimumX; currentX <= maximumX; currentX++)
            {
                if (GetCollisionAtPosition(currentX, currentY) == Tile.Type.COLLISION)
                {
                    frameMovement.Y = currentY * Globals.TileSize.Y - hitbox.Y + hitbox.Height / 2;

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Get the collision type at a certain position.
        /// </summary>
        /// <param name="currentX">The X position to check</param>
        /// <param name="currentY">The Y position to check</param>
        /// <returns>A Tile.Type that says if there is a certain collision here.</returns>
        private Tile.Type GetCollisionAtPosition(int currentX, int currentY)
        {
            int maxX = Map.GetLength(0);
            int maxY = Map.GetLength(1);

            if (currentX > maxX - 1 || currentY > maxY - 1 || currentX < 0 || currentY < 0)
            {
                return Tile.Type.NONE;
            }

            Tile[] tiles = Map[currentX, currentY];

            for (int currentTileIndex = 0; currentTileIndex < tiles.Length; currentTileIndex++)
            {
                Tile currentTile = tiles[currentTileIndex];

                if (!currentTile.TileType.Equals(Tile.Type.NONE))
                {
                    return currentTile.TileType;
                }
            }

            return Tile.Type.NONE;
        }

        /// <summary>
        /// This will load a certain map to use as the tilemap.
        /// </summary>
        /// <param name="fileName">The name of the file you want to load.</param>
        public void LoadMapFromFile(string fileName)
        {
            Map = new Tile[Globals.GameSize.X, Globals.GameSize.Y][];

            // Create a default tile on each x and y coordinate.
            for (int currentX = 0; currentX < Map.GetLength(0); currentX++)
            {
                for (int currentY = 0; currentY < Map.GetLength(1); currentY++)
                {
                    Map[currentX, currentY] = new Tile[10];
                }
            }

            List<int> levelTiles = FileHandler.GetFile(fileName);

            if (levelTiles.Count <= 0)
                return;

            // Create the map with the saved tiles.
            for (int currentLineIndex = 0; currentLineIndex < levelTiles.Count; currentLineIndex += 5)
            {
                int tileX = levelTiles[currentLineIndex];
                int tileY = levelTiles[currentLineIndex + 1];

                int tileIndex = levelTiles[currentLineIndex + 2];

                int tileCollision = levelTiles[currentLineIndex + 3];

                int tileLayer = levelTiles[currentLineIndex + 4];

                Tile newTile = new Tile()
                {
                    TileX = tileX,
                    TileY = tileY,
                    TileIndex = tileIndex,
                    TileType = (Tile.Type)tileCollision,
                    TileLayer = tileLayer
                };

                Map[tileX, tileY][tileLayer] = newTile;
            }
        }

        /// <summary>
        /// This will spawn each pickup located inside the tilemap.
        /// </summary>
        private void SpawnPickups()
		{
            for (int currentX = 0; currentX < Map.GetLength(0); currentX++)
            {
                for (int currentY = 0; currentY < Map.GetLength(1); currentY++)
                {
                    Tile[] currentTiles = Map[currentX, currentY];

					for (int currentTileIndex = 0; currentTileIndex < currentTiles.Length; currentTileIndex++)
					{
                        Tile currentTile = currentTiles[currentTileIndex];

                        if (currentTile.TileIndex == 5)
						{
                            Globals.AllGameObjects.Add(new Coin(new Vector2(currentX * Globals.TileSize.X, currentY * Globals.TileSize.Y)));
						}
                        else if (currentTile.TileIndex == 39)
                        {
                            Globals.AllGameObjects.Add(new Block(new Vector2(currentX * Globals.TileSize.X, currentY * Globals.TileSize.Y), "DIRT_BLOCK"));
                        }
                        else if (currentTile.TileIndex == 56)
                        {
                            Globals.AllGameObjects.Add(new End(new Vector2(currentX * Globals.TileSize.X, currentY * Globals.TileSize.Y)));
                        }
					}
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Visible) 
                return;

            spriteBatch.Begin(transformMatrix: Globals.MainCamera.GetTranslationMatrix());

            for (int currentX = 0; currentX < Map.GetLength(0); currentX++)
            {
                for (int currentY = 0; currentY < Map.GetLength(1); currentY++)
                {
                    for (int currentTileIndex = 0; currentTileIndex < Map[currentX, currentY].Length; currentTileIndex++)
                    {
                        Tile drawTile = Map[currentX, currentY][currentTileIndex];

                        if (drawTile.TileIndex > 0)
                        {
                            spriteBatch.Draw(Globals.MainGraphicsHandler.GetSprite("MainTileSet"), new Rectangle(currentX * Globals.TileSize.X, currentY * Globals.TileSize.Y, Globals.TileSize.X, Globals.TileSize.Y), new Rectangle(drawTile.TileIndex % SheetDimensions.X * Globals.TileSize.X, drawTile.TileIndex / SheetDimensions.X * Globals.TileSize.Y, Globals.TileSize.X, Globals.TileSize.Y), Color.White);
                        }
                    }
                }
            }

            spriteBatch.End();
        }
    }
}
