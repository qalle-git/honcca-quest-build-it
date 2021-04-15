using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonccaBuildingGame.Classes.Extra
{
    class Timer
    {
        public TimeSpan TimerStarted = TimeSpan.Zero;
        public TimeSpan TimerDuration;

        private bool SkipFirst;

        /// <summary>
        /// You choose how long a timer should be and can use it to check between times.
        /// </summary>
        /// <param name="timerInMilliseconds">How long the timer should be specified in milliseconds.</param>
        /// <param name="firstTimeNoTimer">If you want the first timer to be skipped.</param>
        public Timer(float timerInMilliseconds, bool firstTimeNoTimer = false)
        {
            TimerDuration = TimeSpan.FromMilliseconds(timerInMilliseconds);

            SkipFirst = firstTimeNoTimer;
        }

        /// <summary>
        /// Checks whether the timer is finished or not.
        /// </summary>
        /// <returns>A bool that returns true if the timer is finished.</returns>
        /// <param name="currentGameTime">The current gametime object.</param>
        public bool IsFinished(GameTime currentGameTime)
        {
            if (TimerStarted == TimeSpan.Zero)
            {
                TimerStarted = currentGameTime.TotalGameTime;

                if (SkipFirst)
                    return true;
            }

            return currentGameTime.TotalGameTime > TimerStarted + TimerDuration;
        }

        /// <summary>
        /// This will return the time remaining for the current timer.
        /// </summary>
        /// <returns>The time in milliseconds.</returns>
        /// <param name="currentGameTime">The current gametime object.</param>
        public double GetTimeRemaining(GameTime currentGameTime)
        {
            return TimerDuration.TotalMilliseconds - (currentGameTime.TotalGameTime - TimerStarted).TotalMilliseconds;
        }

        /// <summary>
        /// Resets the timer and can now be used again.
        /// </summary>
        /// <param name="currentGameTime">The current gametime object.</param>
        public void ResetTimer(GameTime currentGameTime)
        {
            TimerStarted = currentGameTime.TotalGameTime;
        }

        /// <summary>
        /// Receives the timer in percent.
        /// </summary>
        /// <param name="currentGameTime">The current gametime object.</param>
        /// <returns>The current percent 0-100.</returns>
        public float GetTimerInPercent(GameTime currentGameTime)
        {
            double fadePercent = Math.Clamp((currentGameTime.TotalGameTime.TotalMilliseconds - TimerStarted.TotalMilliseconds) / TimerDuration.TotalMilliseconds * 100, 0, 100);

            return (float)fadePercent;
        }
    }
}
