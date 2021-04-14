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

        private readonly SpriteFont DebugSpriteFont;

        private const string mapName = "LEVEL_ONE";

        private readonly Timer ButtonCooldownTimer = new Timer(150);

        public MapCreator() : base()
        {
            DebugSpriteFont = MainGame.Instance.Content.Load<SpriteFont>("Fonts/mapCreatorFont");
        }

        #region InputControls
        public override void Input(GameTime gameTime)
        {
            if (ButtonCooldownTimer.IsFinished(gameTime))
            {
                KeyboardState keyboardState = Keyboard.GetState();

                MouseState mouseState = Mouse.GetState();

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    ButtonCooldownTimer.ResetTimer(gameTime);

                    Point mousePosition = GetMousePosition();

                    AddToMap(mousePosition.X, mousePosition.Y);
                }
                else if (mouseState.RightButton == ButtonState.Pressed)
                {
                    ButtonCooldownTimer.ResetTimer(gameTime);

                    RemoveTopLayer();
                }

                foreach (Keys key in keyboardState.GetPressedKeys())
                {
                    switch (key)
                    {
                        case Keys.Space:
                            SaveMap();

                            ButtonCooldownTimer.ResetTimer(gameTime);

                            break;
                        case Keys.Up:
                            CurrentTileIndex -= 17;

                            ButtonCooldownTimer.ResetTimer(gameTime);

                            break;
                        case Keys.Left:
                            CurrentTileIndex--;

                            ButtonCooldownTimer.ResetTimer(gameTime);

                            break;
                        case Keys.Down:
                            CurrentTileIndex += 17;

                            ButtonCooldownTimer.ResetTimer(gameTime);

                            break;
                        case Keys.Right:
                            CurrentTileIndex++;

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
                                    Globals.TheTileMap.Map[currentX, currentY] = new Tile[3];
                                }
                            }

                            break;
                        case Keys.K:
                            FillMap();

                            break;
                        case Keys.C:
                            Globals.MainCamera.Move(new Vector2(0, Globals.MainCamera.Position.Y + 10));

                            break;
                        default:
                            break;
                    }
                }
            }
        }
        #endregion

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
                            saveList.Add((int)tiles[currentTileIndex].TileX);
                            saveList.Add((int)tiles[currentTileIndex].TileY);
                            saveList.Add((int)tiles[currentTileIndex].TileIndex);
                            saveList.Add((int)tiles[currentTileIndex].TileType);
                            saveList.Add((int)tiles[currentTileIndex].TileLayer);
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

            int numTilesX = Globals.MainGraphicsHandler.GetSprite("MainTileSet").Width / Globals.TileSize.X;

            Point mousePosition = GetMousePosition();

            Rectangle tileRectangle = new Rectangle(CurrentTileIndex % numTilesX * Globals.TileSize.X, CurrentTileIndex / numTilesX * Globals.TileSize.Y, Globals.TileSize.X, Globals.TileSize.Y);

            if (CurrentTileIndex > 0)
                spriteBatch.Draw(Globals.MainGraphicsHandler.GetSprite("MainTileSet"), new Rectangle(mousePosition.X * Globals.TileSize.X, mousePosition.Y * Globals.TileSize.X, Globals.TileSize.X, Globals.TileSize.Y), tileRectangle, Color.White);

            spriteBatch.Draw(Globals.MainGraphicsHandler.GetSprite("OutlineRectangle"), new Rectangle(mousePosition.X * Globals.TileSize.X, mousePosition.Y * Globals.TileSize.X, Globals.TileSize.X, Globals.TileSize.Y), Color.White);

            spriteBatch.End();

            spriteBatch.Begin();

            spriteBatch.DrawString(DebugSpriteFont, $"X: {mousePosition.X}\nY: {mousePosition.Y}", new Vector2(0, 150), Color.White);
			spriteBatch.DrawString(DebugSpriteFont, $"Tile: {CurrentTileIndex}", new Vector2(0, 60), Color.White);
			spriteBatch.DrawString(DebugSpriteFont, $"Collision: {CollisionTypeIndex} - G - {(Tile.Type)CollisionTypeIndex}", new Vector2(0, 80), Color.White);
			spriteBatch.DrawString(DebugSpriteFont, $"SPACE - SAVE MAP {mapName}", new Vector2(0, 120), Color.White);

            spriteBatch.End();
		}

        private Point GetMousePosition()
        {
            MouseState mouseState = Mouse.GetState();

            int mouseX = (mouseState.Position.X + (int)Globals.MainCamera.Position.X) / Globals.TileSize.X;
            int mouseY = (mouseState.Position.Y + (int)Globals.MainCamera.Position.Y) / Globals.TileSize.Y;

            return new Point(mouseX, mouseY);
        }
    }
}
