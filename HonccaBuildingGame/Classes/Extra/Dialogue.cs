using HonccaBuildingGame.Classes.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HonccaBuildingGame.Classes.Extra
{
    class Dialogue
    {
        private readonly string[] Dialogues;
        private int CurrentDialogue = -1;

        private SpriteFont DialogueFont;

        private TimeSpan StartedDialogue;

        /// <summary>
        /// How long each dialogue will take in milliseconds.
        /// </summary>
        private const int DialogueTime = 4000;

        public Dialogue(string[] dialogues)
        {
            Dialogues = dialogues;

            DialogueFont = MainGame.Instance.Content.Load<SpriteFont>("Fonts/dialogueFont");
        }

        public void StartDialogue(GameTime gameTime)
        {
            StartedDialogue = gameTime.TotalGameTime;

            if (CurrentDialogue >= 0)
			{
                CurrentDialogue++;

                if (CurrentDialogue > Dialogues.Length - 1)
				{
                    CurrentDialogue = 0;
				}
			} 
            else
			{
                CurrentDialogue = 0;
			}
        }

        public string GetDialogueText(GameTime gameTime)
        {
            if (CurrentDialogue < 0)
            {
                return "";
            }
            else if (CurrentDialogue > Dialogues.Length - 1)
			{
                return "";
			}

            string dialogueText = "";

            string currentDialogue = Dialogues[CurrentDialogue];

            // Calculate which char index we're at in the dialogue string.
            double charIndex = Math.Clamp((gameTime.TotalGameTime.TotalMilliseconds - StartedDialogue.TotalMilliseconds) / DialogueTime * currentDialogue.Length, 0, currentDialogue.Length);

            for (int currentStringIndex = 0; currentStringIndex < charIndex; currentStringIndex++)
            {
                dialogueText += currentDialogue[currentStringIndex];
            }

            return dialogueText;
        }

        public int GetAmountOfLines(GameTime gameTime)
        {
            string dialogueText = GetDialogueText(gameTime);

            int amountOfLines = dialogueText.Count(character => character.Equals('\n'));

            return amountOfLines;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            string dialogueText = GetDialogueText(gameTime);

            Vector2 initialPosition = drawPosition - new Vector2(0, Globals.TileSize.Y);

            if (dialogueText.Length > 0)
            {
                Texture2D topSprite = Globals.MainGraphicsHandler.GetSprite("SPEECH_BUBBLE_TOP");

                spriteBatch.Draw(topSprite, new Rectangle((int)initialPosition.X - Globals.TileSize.X / 3, (int)initialPosition.Y - Globals.TileSize.Y / 4, topSprite.Width / 2, 32), Color.White);

                Texture2D middleSprite = Globals.MainGraphicsHandler.GetSprite("SPEECH_BUBBLE_MIDDLE");

                int amountOfLines = GetAmountOfLines(gameTime);

                for (int currentLine = 0; currentLine < amountOfLines; currentLine++)
                {
                    spriteBatch.Draw(middleSprite, new Rectangle((int)initialPosition.X - Globals.TileSize.X / 3, (int)(initialPosition.Y + Globals.TileSize.Y / 4) + (Globals.TileSize.Y / 4 * currentLine), middleSprite.Width / 2, 32), Color.White);
                }

                Texture2D bottomSprite = Globals.MainGraphicsHandler.GetSprite("SPEECH_BUBBLE_BOTTOM");

                spriteBatch.Draw(bottomSprite, new Rectangle((int)initialPosition.X - Globals.TileSize.X / 3, (int)(initialPosition.Y + Globals.TileSize.Y / 4) + (Globals.TileSize.Y / 4 * (amountOfLines > 0 ? (amountOfLines + 1) : 0)), bottomSprite.Width / 2, 48), Color.White);

                spriteBatch.DrawString(DialogueFont, dialogueText, initialPosition, Color.Black);
            }
        }
    }
}
