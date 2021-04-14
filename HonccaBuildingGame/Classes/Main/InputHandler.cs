using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace HonccaBuildingGame.Classes.Main
{
    class InputHandler
    {
        private static KeyboardState CurrentKeyBoardState;
        private static KeyboardState LastKeyBoardState;

        /// <summary>
        /// Refreshes the keyboardstate to help the one-shot function working.
        /// </summary>
        public static void RefreshKeyboardState()
        {
            LastKeyBoardState = CurrentKeyBoardState;

            CurrentKeyBoardState = Keyboard.GetState();
        }

        /// <summary>
        /// This is just a regular IsKeyDown, checks if the key is pressed.
        /// </summary>
        /// <param name="key">The key that you're checking</param>
        /// <returns>If the key is being pressed this frame.</returns>
        public static bool IsBeingPressed(Keys key)
        {
            return CurrentKeyBoardState.IsKeyDown(key);
        }
        
        /// <summary>
        /// Checks whether a key just got pressed, this is for simple one-shot checks.
        /// </summary>
        /// <param name="key">The key that you're checking</param>
        /// <returns>If the key has been pressed this frame.</returns>
        public static bool HasKeyJustBeenPressed(Keys key)
        {
            return CurrentKeyBoardState.IsKeyDown(key) && !LastKeyBoardState.IsKeyDown(key);
        }

        public static Point GetMousePosition()
        {
            MouseState mouseState = Mouse.GetState();

            return mouseState.Position;
        }
    }
}
