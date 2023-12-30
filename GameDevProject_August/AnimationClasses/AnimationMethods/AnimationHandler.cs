using GameDevProject_August.Models.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDevProject_August.AnimationClasses.AnimationMethods
{
    public class AnimationHandler
    {
        public Vector2 Position;
        public Color Colour = Color.White;
        public Vector2 Origin = Vector2.Zero;

        public AnimationHandler()
        {

        }

        public void DrawAnimation(SpriteBatch spriteBatch, Animation animationToRun, Vector2 Position, Direction direction)
        {
            if (direction == Direction.Right)
            {
                spriteBatch.Draw(animationToRun.SpriteSheetTexture, Position, animationToRun.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (direction == Direction.Left)
            {
                spriteBatch.Draw(animationToRun.SpriteSheetTexture, Position, animationToRun.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }
        }

        public void DrawAnimation(SpriteBatch spriteBatch, Animation animationToRun, Vector2 Position, Direction direction, Vector2 offsetAnimation)
        {
            if (direction == Direction.Right)
            {
                spriteBatch.Draw(animationToRun.SpriteSheetTexture, Position + offsetAnimation, animationToRun.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (direction == Direction.Left)
            {
                spriteBatch.Draw(animationToRun.SpriteSheetTexture, Position + offsetAnimation, animationToRun.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }
        }

        public void DrawOneFrameAnimation(SpriteBatch spriteBatch, Texture2D standStillTexture, Vector2 Position, Direction direction)
        {
            if (direction == Direction.Right)
            {
                spriteBatch.Draw(standStillTexture, Position, null, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (direction == Direction.Left)
            {
                spriteBatch.Draw(standStillTexture, Position, null, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }
        }
    }
}
