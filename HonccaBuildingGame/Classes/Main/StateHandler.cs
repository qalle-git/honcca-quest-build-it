using HonccaBuildingGame.Classes.GameObjects;
using HonccaBuildingGame.Classes.GameStates;
using HonccaBuildingGame.Classes.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonccaBuildingGame.Classes.Main
{
	class StateHandler
	{
        private readonly List<GameState> States = new List<GameState>();

        public StateHandler()
        {
            AddState(new SplashScreen());
        }

        public void AddState(GameState newState)
        {
            States.Add(newState);
        }

        public void Input(GameTime gameTime)
        {
            for (int currentStateIndex = 0; currentStateIndex < States.Count; currentStateIndex++)
            {
                States[currentStateIndex].Input(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int currentStateIndex = 0; currentStateIndex < States.Count; currentStateIndex++)
            {
                States[currentStateIndex].Update(gameTime);
            }

            CleanupStates(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int currentStateIndex = States.Count - 1; currentStateIndex >= 0; currentStateIndex--)
            {
                States[currentStateIndex].Draw(gameTime, spriteBatch);
            }
        }

        private void CleanupStates(GameTime gameTime)
        {
            for (int currentStateIndex = States.Count - 1; currentStateIndex >= 0; currentStateIndex--)
            {
                if (States[currentStateIndex].ShouldDispose(gameTime))
                {
                    States.RemoveAt(currentStateIndex);
                }
            }
        }

        public void Clear()
        {
            for (int currentStateIndex = States.Count - 1; currentStateIndex >= 0; currentStateIndex--)
            {
                States.RemoveAt(currentStateIndex);
            }
        }

        /// <summary>
        /// This function will toggle the visiblity on all gamestates except the one specified.
        /// Currently being used inside the map creator hide all current gamestates to only show the map creator.
        /// </summary>
        /// <param name="stateToSkip">Skip this state. (GameState)</param>
        public void ToggleVisibilityOnAllStatesExcept(GameState stateToSkip)
        {
            for (int currentStateIndex = 0; currentStateIndex < States.Count; currentStateIndex++)
            {
                GameState currentState = States[currentStateIndex];

                if (currentState != stateToSkip)
                {
                    currentState.Visible = !currentState.Visible;
                }
            }
        }
    }
}
