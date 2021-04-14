using HonccaBuildingGame.Classes.Extra;
using HonccaBuildingGame.Classes.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonccaBuildingGame.Classes.GameObjects
{
    class NPC : Animation
    {
        public Dialogue NPCDialogue;

        public NPC(Vector2 position, Texture2D texture, string[] dialogue) : base(position, texture)
        {
            NPCDialogue = new Dialogue(dialogue);

            PhysicsEnabled = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            NPCDialogue.Draw(gameTime, spriteBatch, Position);
        }
    }
}
