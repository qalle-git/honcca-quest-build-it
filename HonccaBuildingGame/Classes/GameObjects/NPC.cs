using HonccaBuildingGame.Classes.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HonccaBuildingGame.Classes.GameObjects
{
    class NPC : Animation
    {
        // The dialogue connected to the npc.
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
