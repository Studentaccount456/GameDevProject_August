using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDevProject_August.Sprites.DNotSentient
{
    abstract public class NotSentient : Sprite
    {
        public bool IsDestroyed = false;

        public Texture2D staticTexture;

        public override Rectangle RectangleHitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, staticTexture.Width, staticTexture.Height);
            }
        }

        public NotSentient(Texture2D texture) : base()
        {
            staticTexture = texture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(staticTexture, Position, Color.White);

            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
        }
    }
}
