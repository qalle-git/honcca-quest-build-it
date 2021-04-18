using HonccaBuildingGame.Classes.GameObjects;
using HonccaBuildingGame.Classes.GameStates;
using HonccaBuildingGame.Classes.Main;
using Microsoft.Xna.Framework;

namespace HonccaBuildingGame.Classes.Pickups
{
    /// <summary>
    /// If this is picked up the game will end.
    /// </summary>
    class End : Pickup
    {
        public End(Vector2 _startPosition) : base(_startPosition, Globals.MainGraphicsHandler.GetSprite("OutlineRectangle"))
        {
        }

        public override void OnPickup(GameTime gameTime)
        {
            EndScreen endScreen = new EndScreen();

            Globals.TheStateMachine.AddState(endScreen);
        }
    }
}
