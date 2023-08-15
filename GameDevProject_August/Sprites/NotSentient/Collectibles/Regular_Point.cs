using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDevProject_August.Sprites.NotSentient.Collectibles
{
    //TODO: Implement the class
    internal class Regular_Point : Sprite
    {
        public Regular_Point(Texture2D texture)
            : base(texture)
        {
            _texture = texture;
        }

        public override Rectangle RectangleHitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }
    }
}
