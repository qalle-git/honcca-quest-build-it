using HonccaBuildingGame.Classes.Extra;
using HonccaBuildingGame.Classes.Main;
using HonccaBuildingGame.Classes.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;

namespace HonccaBuildingGame.Classes.GameStates
{
    class EndScreen : GameState
    {
        private TimeSpan FinishedGame = TimeSpan.Zero;

        private readonly Timer FadeTimer = new Timer(1500);

        private bool Reverse = false;

        // This is how many coins the player have collected in total.
        private readonly int CoinCount = 0;

        public EndScreen()
        {
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.Play(MainGame.MainMenuSong);

            CoinCount = Globals.MainPlayer.ItemInventory.GetItemWithName("COIN").Count;

            Globals.TheStateMachine.Clear();
            Globals.AllGameObjects.Clear();

            Globals.TheStateMachine.AddState(this);

            Globals.TheTileMap = new TileMap("END_SCREEN");
            Globals.TheStateMachine.AddState(Globals.TheTileMap);
            Globals.MainCamera.ForceMove(Vector2.Zero);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (FinishedGame == TimeSpan.Zero)
            {
                FinishedGame = gameTime.TotalGameTime - MainGame.GameStarted;
            }

            if (FadeTimer.IsFinished(gameTime))
            {
                FadeTimer.ResetTimer(gameTime);

                Reverse = !Reverse;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            float fadePercent = FadeTimer.GetTimerInPercent(gameTime);

            float currentPercent = (Reverse ? (1 - fadePercent / 100) : fadePercent / 100);

            const string keyString = "Press R to restart";

            spriteBatch.DrawString(SplashScreen.PressAnyKeyFont, keyString, new Vector2(Globals.ScreenSize.X / 2 - SplashScreen.PressAnyKeyFont.MeasureString(keyString).X / 2, Globals.ScreenSize.Y / 6 * 5), Color.Black * currentPercent);

            int minutesLeft = (int)FinishedGame.TotalSeconds / 60;
            int secondsLeft = (int)FinishedGame.TotalSeconds % 60;

            string timeString = $"You finished in {minutesLeft} {(minutesLeft == 1 ? "minute" : "minutes")} and {secondsLeft} {(secondsLeft == 1 ? "second" : "seconds")}";

            spriteBatch.DrawString(SplashScreen.PressAnyKeyFont, timeString, new Vector2(Globals.ScreenSize.X / 2 - SplashScreen.PressAnyKeyFont.MeasureString(timeString).X / 2, Globals.ScreenSize.Y / 6 * 4), Color.Black);

            string coinString = $"You collected a total of {CoinCount} {(CoinCount > 1 ? "coins" : "coin")}";

            spriteBatch.DrawString(SplashScreen.PressAnyKeyFont, coinString, new Vector2(Globals.ScreenSize.X / 2 - SplashScreen.PressAnyKeyFont.MeasureString(coinString).X / 2, Globals.ScreenSize.Y / 6 * 4.5f), Color.Black);

            const string gameNameString = "Build It!";

            spriteBatch.DrawString(SplashScreen.TitleFont, gameNameString, new Vector2(Globals.ScreenSize.X / 2 - SplashScreen.TitleFont.MeasureString(gameNameString).X / 2, Globals.TileSize.X * 2 - SplashScreen.TitleFont.MeasureString(gameNameString).Y / 2), Color.Black);

            spriteBatch.End();
        }
    }
}
