using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace HonccaBuildingGame.Classes.Main
{
    public class GraphicsHandler
    {
        public Dictionary<string, Sprite> Graphics = new Dictionary<string, Sprite>()
        {
            {
                "MainTileSet",

                new Sprite()
                {
                    FileName = "Tiles/mainSet"
                }
            },
            {
                "OutlineRectangle",

                new Sprite()
                {
                    FileName = "Sprites/outlineRectangle"
                }
            },
            {
                "InventorySlot",

                new Sprite()
                {
                    FileName = "Sprites/inventorySlot"
                }
            },
            {
                "SelectedInventorySlot",

                new Sprite()
                {
                    FileName = "Sprites/inventorySlotSelected"
                }
            },
            {
                "BobTheBuilder",

                new Sprite()
                {
                    FileName = "SpriteSheets/idleSpriteSheet"
                }
            },
            {
                "NPC_ONE",

                new Sprite()
                {
                    FileName = "SpriteSheets/npcOneSpritesheet"
                }
            },
			{
                "Coin",

                new Sprite()
				{
                    FileName = "SpriteSheets/coinSpritesheet"
				}
			},
            {
                "COIN",

                new Sprite()
                {
                    FileName = "Sprites/coinItem"
                }
            },
            {
                "DIRT_BLOCK",

                new Sprite()
                {
                    FileName = "Sprites/groundItem"
                }
            },
            {
                "DIALOGUE_INTERACT",

                new Sprite()
                {
                    FileName = "Sprites/interactNPC"
                }
            },
            {
                "StaticBox",

                new Sprite()
                {
                    FileName = "SpriteSheets/boxIdle"
                }
            },
            {
                "Door",

                new Sprite()
                {
                    FileName = "SpriteSheets/doorAnimation"
                }
            },
            {
                "BUTTON_E",

                new Sprite()
                {
                    FileName = "Sprites/eButton"
                }
            },
            {
                "BUTTON_F",

                new Sprite()
                {
                    FileName = "Sprites/fButton"
                }
            },
            {
                "BUTTON_R",

                new Sprite()
                {
                    FileName = "Sprites/rButton"
                }
            }
        };

        public GraphicsHandler()
        {
            Dictionary<string, Sprite> newGraphics = Graphics;

            foreach (var graphic in newGraphics)
                Graphics[graphic.Key].LoadTexture();
        }

        /// <summary>
        /// Get the Texture2D object.
        /// </summary>
        /// <param name="spriteName">The spriteName</param>
        /// <returns>A texture2d object.</returns>
        public Texture2D GetSprite(string spriteName)
        {
            if (Graphics.ContainsKey(spriteName))
            {
                if (Graphics[spriteName].Texture == null)
                    throw new Exception($"{spriteName} doesn't exist in the Content folder.");

                return Graphics[spriteName].Texture;
            }

            throw new Exception($"{spriteName} doesn't exist in the dictionary.");
        }

        /// <summary>
        /// Get the spriteName from a Texture2D object.
        /// </summary>
        /// <param name="texture">The Texture2D object.</param>
        /// <returns></returns>
        public string GetSpriteNameFromTexture2D(Texture2D texture)
        {
            foreach (var currentSprite in Graphics)
            {
                if (currentSprite.Value.Texture == texture)
                {
                    return currentSprite.Key;
                }
            }

            return string.Empty;
        }
    }

    public class Sprite
    {
        public string FileName;

        public Texture2D Texture;

        public void LoadTexture()
        {
            Texture = MainGame.Instance.Content.Load<Texture2D>(FileName);
        }
    }
}
