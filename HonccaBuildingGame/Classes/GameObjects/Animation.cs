using HonccaBuildingGame.Classes.Main;
using HonccaBuildingGame.Classes.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonccaBuildingGame.Classes.GameObjects
{
    class Animation : GameObject
    {
        public float AnimationSpeed;

        public Point TotalFrames;
        public Point CurrentFrame;

        public Point FrameRange;

        public int Multiplier;

        public bool FullAnimation;

        public State CurrentState = State.IDLE;
        public Direction CurrentDirection;

        public Point TileSize = Globals.TileSize;

        /// <summary>
        /// The direction the animation will face, LEFT will flip the object horizontally.
        /// </summary>
        public enum Direction
        {
            LEFT,
            RIGHT
        }

        /// <summary>
        /// The state of the animation object, ANIMATING = the object will animate. IDLE = the object will not animate.
        /// </summary>
        public enum State
        {
            ANIMATING,
            IDLE
        }

        public Animation(Vector2 position, Texture2D texture) : base(position, texture)
        {
        }

        /// <summary>
        /// Receive the hitbox of the animation.
        /// </summary>
        /// <returns>The hitbox in a rectangle object.</returns>
        public override Rectangle GetRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Globals.TileSize.X - 4, TileSize.Y);
        }
     

        /// <summary>
        /// Set the animation data to this object.
        /// </summary>
        /// <param name="_totalFrames">How many frames the total sprite is | X | Y |</param>
        /// <param name="_frameRange">The range this object will take use of in the X-axis</param>
        /// <param name="_direction">The direction the sprite should face | LEFT | RIGHT |</param>
        /// <param name="_animationSpeed">Animation speed.</param>
        /// <param name="_multiplier">Scale multiplier.</param>
        public void SetAnimationData(Point _totalFrames, Point _frameRange, Direction _direction, float _animationSpeed = 120f, int _multiplier = 1, bool _fullAnimation = false)
        {
            TotalFrames = _totalFrames;
            FrameRange = _frameRange;

            CurrentFrame.X = FrameRange.X;

            CurrentDirection = _direction;

            AnimationSpeed = _animationSpeed;

            Multiplier = _multiplier;
            FullAnimation = _fullAnimation;

            AnimationCooldown = TimeSpan.FromMilliseconds(AnimationSpeed);
        }

        public TimeSpan AnimationCooldown;
        public TimeSpan LastAnimation = TimeSpan.Zero;

        public override void Update(GameTime gameTime)
        {
            if (!Active)
                return;

            if (CurrentState == State.ANIMATING)
			{
                if (gameTime.TotalGameTime > LastAnimation + AnimationCooldown)
                {
                    int newFrame = CurrentFrame.X + 1;

                    if (newFrame < FrameRange.Y)
					{
                        CurrentFrame.X++;
					}
                    else
                    {
                        CurrentFrame.X = FrameRange.X;

                        if (FullAnimation)
                        {
                            int newFrameY = CurrentFrame.Y + 1;

                            if (newFrameY < TotalFrames.Y + 1)
							{
                                CurrentFrame.Y++;
							}
                            else
							{
                                CurrentFrame.Y = 0;
							}
                        }
                    }

                    LastAnimation = gameTime.TotalGameTime;
                }
			}

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Active)
                return;

            int tileSizeY = TileSize.Y;

            Rectangle drawRectangle = new Rectangle((int)Position.X, (int)Position.Y, Globals.TileSize.X * Multiplier, tileSizeY * Multiplier);
            Rectangle sourceRectangle = new Rectangle(CurrentFrame.X * Globals.TileSize.X, CurrentFrame.Y * tileSizeY, Globals.TileSize.X, tileSizeY);

            spriteBatch.Draw(Texture, drawRectangle, sourceRectangle, Color.White, 0f, Vector2.Zero, CurrentDirection == Direction.LEFT ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
