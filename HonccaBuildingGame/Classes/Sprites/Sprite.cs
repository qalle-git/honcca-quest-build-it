using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonccaBuildingGame.Classes.Sprites
{
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
