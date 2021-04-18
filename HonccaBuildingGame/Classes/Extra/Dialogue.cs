using HonccaBuildingGame.Classes.GameObjects;
using HonccaBuildingGame.Classes.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        public int CurrentDialogue = -1;

        public static SpriteFont DialogueFont;

        private readonly SoundEffect TypingSoundEffect;
        private readonly Timer TypingSoundTimer;

        private TimeSpan StartedDialogue;

        private readonly Animation InteractAnimation;

        public int DialogueCount
        {
            get
            {
                return Dialogues.Length;
            }
        }


        /// <summary>
        /// How long each dialogue will take in milliseconds.
        /// </summary>
        private const int DialogueTime = 2000;

        public Dialogue(string[] dialogues)
        {
            Dialogues = dialogues;

            DialogueFont = MainGame.Instance.Content.Load<SpriteFont>("Fonts/pixelFont");

            TypingSoundEffect = Globals.MainAudioHandler.GetAudio("DIALOGUE");
            TypingSoundTimer = new Timer((float)TypingSoundEffect.Duration.TotalMilliseconds, true);

            InteractAnimation = new Animation(Vector2.Zero, Globals.MainGraphicsHandler.GetSprite("DIALOGUE_INTERACT"));
            InteractAnimation.SetAnimationData(new Point(4, 0), new Point(0, 4), Animation.Flip.RIGHT, 280);
            InteractAnimation.CurrentState = Animation.State.ANIMATING;
        }

        /// <summary>
        /// Start the dialogue.
        /// </summary>
        /// <param name="gameTime">The current gameTime object.</param>
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

        /// <summary>
        /// Get the current dialogue text.
        /// </summary>
        /// <param name="gameTime">The current gameTime object.</param>
        /// <returns>A string with the dialogue text.</returns>
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

            double charIndex = GetPercentageOfDialogue(gameTime);

            string currentDialogue = Dialogues[CurrentDialogue];

            for (int currentStringIndex = 0; currentStringIndex < charIndex; currentStringIndex++)
            {
                dialogueText += currentDialogue[currentStringIndex];
            }

            return dialogueText;
        }

        /// <summary>
        /// How many lines there are inside the dialogue text.
        /// </summary>
        /// <param name="gameTime">The current gameTime object.</param>
        /// <returns>A int which determines the lines inside the dialogue.</returns>
        public int GetAmountOfLines(GameTime gameTime)
        {
            string dialogueText = GetDialogueText(gameTime);

            int amountOfLines = dialogueText.Count(character => character.Equals('\n'));

            return amountOfLines;
        }

        /// <summary>
        /// Get how much of the dialogue that is done in %.
        /// </summary>
        /// <param name="gameTime">The current gameTime object.</param>
        /// <returns>A double which is the % fo the dialogue.</returns>
        private double GetPercentageOfDialogue(GameTime gameTime)
        {
            string currentDialogue = Dialogues[CurrentDialogue];

            // Calculate which char index we're at in the dialogue string.
            double percentage = Math.Clamp((gameTime.TotalGameTime.TotalMilliseconds - StartedDialogue.TotalMilliseconds) / DialogueTime * currentDialogue.Length, 0, currentDialogue.Length);

            return percentage;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            string dialogueText = GetDialogueText(gameTime);

            Vector2 initialPosition = drawPosition - new Vector2(0, Globals.TileSize.Y);

            if (dialogueText.Length > 0)
            {
                double charIndex = GetPercentageOfDialogue(gameTime);

                if (charIndex < dialogueText.Length && TypingSoundTimer.IsFinished(gameTime))
                {
                    Globals.MainAudioHandler.PlaySound("DIALOGUE", 0.07f);

                    TypingSoundTimer.ResetTimer(gameTime);
                }

                spriteBatch.DrawString(DialogueFont, dialogueText, new Vector2(Globals.ScreenSize.X - DialogueFont.MeasureString(dialogueText).X + Globals.MainCamera.Position.X, 0), Color.White);
            }
            else
            {
                InteractAnimation.Update(gameTime);

                InteractAnimation.Position = initialPosition;

                InteractAnimation.Draw(gameTime, spriteBatch);
            }
        }
    }
}
