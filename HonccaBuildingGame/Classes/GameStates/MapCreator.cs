using HonccaBuildingGame.Classes.Extra;
using HonccaBuildingGame.Classes.Main;
using HonccaBuildingGame.Classes.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonccaBuildingGame.Classes.GameStates
{
    class MapCreator : GameState
    {
        private int CurrentTileIndex;
        private int CollisionTypeIndex;

        private bool InventoryViewOpen = false;

        private readonly SpriteFont DebugSpriteFont;

        private const string mapName = "LEVEL_ONE";

        private readonly Timer ButtonCooldownTimer = new Timer(150);

        public MapCreator() : base()
        {
            DebugSpriteFont = MainGame.Instance.Content.Load<SpriteFont>("Fonts/mapCreatorFont");
        }

        #region InputControls

        private int LastScrollValue;

        public override void Input(GameTime gameTime)
        {
            if (ButtonCooldownTimer.IsFinished(gameTime))
            {
                KeyboardState keyboardState = Keyboard.GetState();

                MouseState mouseState = Mouse.GetState();

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    ButtonCooldownTimer.ResetTimer(gameTime);

                    if (InventoryViewOpen)
                    {
                        Point mousePosition = GetMousePosition(true);

                        CurrentTileIndex = mousePosition.Y > 0 ? mousePosition.X + (mousePosition.Y * 17) : mousePosition.X;
                    }
                    else
                    {
                        Point mousePosition = GetMousePosition();

                        AddToMap(mousePosition.X, mousePosition.Y);
                    }
                }
                else if (mouseState.RightButton == ButtonState.Pressed)
                {
                    ButtonCooldownTimer.ResetTimer(gameTime);

                    RemoveTopLayer();
                }
                else if (mouseState.ScrollWheelValue != LastScrollValue)
                {
                    bool shouldGoUp = mouseState.ScrollWheelValue > LastScrollValue;

                    Globals.MainCamera.Move(new Vector2(0, Globals.MainCamera.Position.Y + (shouldGoUp ? 100 : -100)));
                }
                else if (mouseState.MiddleButton == ButtonState.Pressed)
                {
                    ButtonCooldownTimer.ResetTimer(gameTime);

                    CopyTopLayer();
                }

                LastScrollValue = mouseState.ScrollWheelValue;

                foreach (Keys key in keyboardState.GetPressedKeys())
                {
                    switch (key)
                    {
                        case Keys.Space:
                            SaveMap();

                            ButtonCooldownTimer.ResetTimer(gameTime);

                            break;
                        case Keys.G:
                            CollisionTypeIndex++;

                            ButtonCooldownTimer.ResetTimer(gameTime);

                            break;
                        case Keys.F:
                            CollisionTypeIndex--;

                            ButtonCooldownTimer.ResetTimer(gameTime);

                            break;
                        case Keys.Delete:
                            Globals.TheTileMap.Map = new Tile[Globals.GameSize.X, Globals.GameSize.Y][];

                            for (int currentX = 0; currentX < Globals.TheTileMap.Map.GetLength(0); currentX++)
                            {
                                for (int currentY = 0; currentY < Globals.TheTileMap.Map.GetLength(1); currentY++)
                                {
                                    Globals.TheTileMap.Map[currentX, currentY] = new Tile[10];
                                }
                            }

                            break;
                        case Keys.K:
                            FillMap();

                            break;
                        case Keys.Tab:
                            InventoryViewOpen = !InventoryViewOpen;

                            Globals.TheStateMachine.ToggleVisibilityOnAllStatesExcept(this);

                            ButtonCooldownTimer.ResetTimer(gameTime);

                            break;
                        default:
                            break;
                    }
                }
            }
        }
        #endregion

        private void CopyTopLayer()
        {
            Point mousePosition = GetMousePosition();

            Tile[] tilesAtPosition = Globals.TheTileMap.Map[mousePosition.X, mousePosition.Y];

            for (int currentTileIndex = tilesAtPosition.Length - 1; currentTileIndex >= 0; currentTileIndex--)
            {
                Tile currentTile = tilesAtPosition[currentTileIndex];

                if (currentTile.TileIndex > 0)
                {
                    CurrentTileIndex = currentTile.TileIndex;

                    break;
                }
            }
        }

        private void FillMap()
        {
            for (int currentX = 0; currentX < Globals.TheTileMap.Map.GetLength(0); currentX++)
            {
                for (int currentY = 0; currentY < Globals.TheTileMap.Map.GetLength(1); currentY++)
                {
                    Tile[] tiles = Globals.TheTileMap.Map[currentX, currentY];

                    tiles[0] = new Tile()
                    {
                        TileX = currentX,
                        TileY = currentY,
                        TileIndex = CurrentTileIndex,
                        TileType = Tile.Type.NONE,
                        TileLayer = 0
                    };
                }
            }
        }

        private void SaveMap()
        {
            List<int> saveList = new List<int>();

            for (int currentX = 0; currentX < Globals.TheTileMap.Map.GetLength(0); currentX++)
            {
                for (int currentY = 0; currentY < Globals.TheTileMap.Map.GetLength(1); currentY++)
                {
                    Tile[] tiles = Globals.TheTileMap.Map[currentX, currentY];

                    for (int currentTileIndex = 0; currentTileIndex < tiles.Length; currentTileIndex++)
                    {
                        if (tiles[currentTileIndex].TileIndex > 0)
                        {
                            saveList.Add(tiles[currentTileIndex].TileX);
                            saveList.Add(tiles[currentTileIndex].TileY);
                            saveList.Add(tiles[currentTileIndex].TileIndex);
                            saveList.Add((int)tiles[currentTileIndex].TileType);
                            saveList.Add(tiles[currentTileIndex].TileLayer);
                        }
                    }
                }
            }
 
            FileHandler.AddFile(mapName, saveList);

            Console.WriteLine($"Saved Map: {mapName}");
        }

        private void AddToMap(float positionX = -1, float positionY = -1)
        {
            if (positionX < 0)
                return;
            else if (positionX > Globals.GameSize.X - 1)
                return;
            else if (positionY < 0)
                return;
            else if (positionY > Globals.GameSize.Y - 1)
                return;

            Tile[] tiles = Globals.TheTileMap.Map[(int)positionX, (int)positionY];

            int freeLayer = -1;

            for (int currentTileIndex = 0; currentTileIndex < tiles.Length; currentTileIndex++)
            {
                if (tiles[currentTileIndex].TileIndex == 0)
                {
                    freeLayer = currentTileIndex;

                    break;
                }
            }

            if (freeLayer == -1)
                return;

            Tile newTile = new Tile()
            {
                TileX = (int)positionX,
                TileY = (int)positionY,
                TileIndex = CurrentTileIndex,
                TileType = (Tile.Type)CollisionTypeIndex,
                TileLayer = freeLayer
            };

            tiles[freeLayer] = newTile;
        }

        private void RemoveTopLayer()
        {
            Point mousePosition = GetMousePosition();

            if (mousePosition.X < 0)
                return;
            else if (mousePosition.X > Globals.GameSize.X - 1)
                return;
            else if (mousePosition.Y < 0)
                return;
            else if (mousePosition.Y > Globals.GameSize.Y - 1)
                return;

            Tile[] tiles = Globals.TheTileMap.Map[mousePosition.X, mousePosition.Y];

            for (int currentTileIndex = tiles.Length - 1; currentTileIndex >= 0; currentTileIndex--)
            {
                if (tiles[currentTileIndex].TileIndex > 0)
                {
                    Globals.TheTileMap.Map[mousePosition.X, mousePosition.Y][currentTileIndex] = new Tile();

                    break;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.Begin(transformMatrix: Globals.MainCamera.GetTranslationMatrix());

            Point mousePosition = GetMousePosition();

            Texture2D tileSetTexture = Globals.MainGraphicsHandler.GetSprite("MainTileSet");

            if (!InventoryViewOpen)
            {
                int numTilesX = tileSetTexture.Width / Globals.TileSize.X;

                Rectangle tileRectangle = new Rectangle(CurrentTileIndex % numTilesX * Globals.TileSize.X, CurrentTileIndex / numTilesX * Globals.TileSize.Y, Globals.TileSize.X, Globals.TileSize.Y);

                if (CurrentTileIndex > 0)
                    spriteBatch.Draw(tileSetTexture, new Rectangle(mousePosition.X * Globals.TileSize.X, mousePosition.Y * Globals.TileSize.X, Globals.TileSize.X, Globals.TileSize.Y), tileRectangle, Color.White);
            }

            spriteBatch.Draw(Globals.MainGraphicsHandler.GetSprite("OutlineRectangle"), new Rectangle(mousePosition.X * Globals.TileSize.X, mousePosition.Y * Globals.TileSize.X, Globals.TileSize.X, Globals.TileSize.Y), Color.White);

            spriteBatch.End();

            spriteBatch.Begin();

            if (InventoryViewOpen)
            {
                spriteBatch.Draw(tileSetTexture, new Rectangle(0, 0, tileSetTexture.Width, tileSetTexture.Height), Color.White);
            }

            spriteBatch.DrawString(DebugSpriteFont, $"X: {mousePosition.X}\nY: {mousePosition.Y}", new Vector2(0, 150), Color.White);
			spriteBatch.DrawString(DebugSpriteFont, $"Tile: {CurrentTileIndex}", new Vector2(0, 60), Color.White);
			spriteBatch.DrawString(DebugSpriteFont, $"Collision: {CollisionTypeIndex} - G - {(Tile.Type)CollisionTypeIndex}", new Vector2(0, 80), Color.White);
			spriteBatch.DrawString(DebugSpriteFont, $"SPACE - SAVE MAP {mapName}", new Vector2(0, 120), Color.White);

            spriteBatch.End();
		}

        private Point GetMousePosition(bool noCamera = false)
        {
            MouseState mouseState = Mouse.GetState();

            if (noCamera)
            {
                return new Point(mouseState.Position.X / Globals.TileSize.X, mouseState.Position.Y / Globals.TileSize.Y);
            }

            int mouseX = (mouseState.Position.X + (int)Globals.MainCamera.Position.X) / Globals.TileSize.X;
            int mouseY = (mouseState.Position.Y + (int)Globals.MainCamera.Position.Y) / Globals.TileSize.Y;

            return new Point(mouseX, mouseY);
        }
    }
}
