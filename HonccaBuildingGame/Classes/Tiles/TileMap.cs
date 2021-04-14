using HonccaBuildingGame.Classes.Main;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using HonccaBuildingGame.Classes.Extra;
using HonccaBuildingGame.Classes.Pickups;
using HonccaBuildingGame.Classes.GameObjects;

namespace HonccaBuildingGame.Classes.Tiles
{
    class TileMap : GameState
    {
        public Tile[,][] Map;

        public TileMap(string fileName)
        {
            LoadMapFromFile(fileName);

            SpawnPickups();
        }

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
                            Furniture staticBox = new Furniture(new Vector2(currentX * Globals.TileSize.X, currentY * Globals.TileSize.Y), Globals.MainGraphicsHandler.GetSprite("StaticBox"));

                            staticBox.SetAnimationData(new Point(2, 0), new Point(0, 2), Animation.Direction.LEFT, 450);

                            Globals.AllGameObjects.Add(staticBox);
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

            int numTilesX = Globals.MainGraphicsHandler.GetSprite("MainTileSet").Width / Globals.TileSize.X;

            for (int currentX = 0; currentX < Map.GetLength(0); currentX++)
            {
                for (int currentY = 0; currentY < Map.GetLength(1); currentY++)
                {
                    for (int currentTileIndex = 0; currentTileIndex < Map[currentX, currentY].Length; currentTileIndex++)
                    {
                        Tile drawTile = Map[currentX, currentY][currentTileIndex];

                        if (drawTile.TileIndex > 0)
                        {
                            spriteBatch.Draw(Globals.MainGraphicsHandler.GetSprite("MainTileSet"), new Rectangle(currentX * Globals.TileSize.X, currentY * Globals.TileSize.Y, Globals.TileSize.X, Globals.TileSize.Y), new Rectangle(drawTile.TileIndex % numTilesX * Globals.TileSize.X, drawTile.TileIndex / numTilesX * Globals.TileSize.Y, Globals.TileSize.X, Globals.TileSize.Y), Color.White);
                        }
                    }
                }
            }

            spriteBatch.End();
        }
    }
}
