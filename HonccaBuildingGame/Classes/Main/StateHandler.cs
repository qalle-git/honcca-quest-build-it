using HonccaBuildingGame.Classes.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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

        /// <summary>
        /// This method checks whether any gamestate needs to be deleted, if so it deletes it.
        /// </summary>
        /// <param name="gameTime">The current gameTime object.</param>
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

        /// <summary>
        /// This will clear all current states active.
        /// </summary>
        public void Clear()
        {
            States.Clear();
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
