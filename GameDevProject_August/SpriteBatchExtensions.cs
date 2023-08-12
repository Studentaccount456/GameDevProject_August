using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDevProject_August
{
    public static class SpriteBatchExtensions
    {
        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color, int thickness = 1)
        {
            // Draw top
            spriteBatch.DrawPixel(new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
            // Draw left
            spriteBatch.DrawPixel(new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
            // Draw right
            spriteBatch.DrawPixel(new Rectangle(rectangle.X + rectangle.Width - thickness, rectangle.Y, thickness, rectangle.Height), color);
            // Draw bottom
            spriteBatch.DrawPixel(new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - thickness, rectangle.Width, thickness), color);
        }

        public static void DrawPixel(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            Texture2D pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { color });
            spriteBatch.Draw(pixel, rectangle, color);
        }
    }
}