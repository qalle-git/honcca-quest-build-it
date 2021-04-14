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
        private readonly List<IGameState> States = new List<IGameState>();

        public StateHandler()
        {
            AddState(new SplashScreen());
        }

        public void AddState(IGameState newState)
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

            CleanupStates();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int currentStateIndex = States.Count - 1; currentStateIndex >= 0; currentStateIndex--)
            {
                States[currentStateIndex].Draw(gameTime, spriteBatch);
            }
        }

        private void CleanupStates()
        {
            for (int currentStateIndex = States.Count - 1; currentStateIndex >= 0; currentStateIndex--)
            {
                if (States[currentStateIndex].ShouldDispose())
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
    }
}
