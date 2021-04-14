using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

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
                "SPEECH_BUBBLE_TOP",

                new Sprite()
                {
                    FileName = "Sprites/speechBubbleTop"
                }
            },
            {
                "SPEECH_BUBBLE_MIDDLE",

                new Sprite()
                {
                    FileName = "Sprites/speechBubbleMiddle"
                }
            },
            {
                "SPEECH_BUBBLE_BOTTOM",

                new Sprite()
                {
                    FileName = "Sprites/speechBubbleBottom"
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
            }
        };

        public GraphicsHandler()
        {
            Dictionary<string, Sprite> newGraphics = Graphics;

            foreach (var graphic in newGraphics)
                Graphics[graphic.Key].LoadTexture();
        }

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
