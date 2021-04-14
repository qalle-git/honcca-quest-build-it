using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace HonccaBuildingGame.Classes.Sprites
{
    public class SpriteHandler
    {
        public Dictionary<string, Sprite> AllSprites = new Dictionary<string, Sprite>()
        {
            {
                "HonccaLogo",

                new Sprite()
                {
                    FileName = "Sprites/honccaLogo"
                }
            }
        };

        public SpriteHandler()
        {
            Dictionary<string, Sprite> newGraphics = AllSprites;

            foreach (var graphic in newGraphics)
                AllSprites[graphic.Key].LoadTexture();
        }

        public Texture2D GetSprite(string spriteName)
        {
            if (AllSprites.ContainsKey(spriteName))
            {
                if (AllSprites[spriteName].Texture == null)
                    throw new Exception($"{spriteName} doesn't exist in the Content folder.");

                return AllSprites[spriteName].Texture;
            }

            throw new Exception($"{spriteName} doesn't exist in the dictionary.");
        }
    }
}
