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

        public void DrawAnimation(SpriteBatch spriteBatch, Animation animationToRun, Vector2 Position, bool faceRight)
        {
            if (faceRight == true)
            {
                spriteBatch.Draw(animationToRun.SpriteSheetTexture, Position, animationToRun.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(animationToRun.SpriteSheetTexture, Position, animationToRun.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }
        }

        public void DrawOneFrameAnimation(SpriteBatch spriteBatch, Texture2D standStillTexture, Vector2 Position, bool faceRight)
        {
            if (faceRight == true)
            {
                spriteBatch.Draw(standStillTexture, Position, null, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(standStillTexture, Position, null, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }
        }
    }
}
