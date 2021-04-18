using HonccaBuildingGame.Classes.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HonccaBuildingGame.Classes.GameObjects
{
    class Animation : GameObject
    {
        protected float AnimationSpeed;

        protected Point TotalFrames;
        public Point CurrentFrame;

        protected Point FrameRange;

        protected int Multiplier;

        protected bool FullAnimation;

        public State CurrentState = State.IDLE;
        public Flip TextureDirection;

        public Point TextureSize = Globals.TileSize;

        /// <summary>
        /// The direction the animation will face, LEFT will flip the object horizontally.
        /// </summary>
        public enum Flip
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
            return new Rectangle((int)Position.X, (int)Position.Y, Globals.TileSize.X - 4, TextureSize.Y);
        }
     

        /// <summary>
        /// Set the animation data to this object.
        /// </summary>
        /// <param name="_totalFrames">How many frames the total sprite is | X | Y |</param>
        /// <param name="_frameRange">The range this object will take use of in the X-axis</param>
        /// <param name="_direction">The direction the sprite should face | LEFT | RIGHT |</param>
        /// <param name="_animationSpeed">Animation speed.</param>
        /// <param name="_multiplier">Scale multiplier.</param>
        public void SetAnimationData(Point _totalFrames, Point _frameRange, Flip _direction, float _animationSpeed = 120f, int _multiplier = 1, bool _fullAnimation = false)
        {
            TotalFrames = _totalFrames;
            FrameRange = _frameRange;

            CurrentFrame.X = FrameRange.X;

            TextureDirection = _direction;

            AnimationSpeed = _animationSpeed;

            Multiplier = _multiplier;
            FullAnimation = _fullAnimation;

            AnimationCooldown = TimeSpan.FromMilliseconds(AnimationSpeed);
        }

        public TimeSpan AnimationCooldown;
        public TimeSpan LastAnimation = TimeSpan.Zero;

        public override void Update(GameTime gameTime)
        {
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
            int tileSizeY = TextureSize.Y;

            Rectangle drawRectangle = new Rectangle((int)Position.X, (int)Position.Y, Globals.TileSize.X * Multiplier, tileSizeY * Multiplier);
            Rectangle sourceRectangle = new Rectangle(CurrentFrame.X * Globals.TileSize.X, CurrentFrame.Y * tileSizeY, Globals.TileSize.X, tileSizeY);

            spriteBatch.Draw(Texture, drawRectangle, sourceRectangle, Color.White, 0f, Vector2.Zero, TextureDirection == Flip.LEFT ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
