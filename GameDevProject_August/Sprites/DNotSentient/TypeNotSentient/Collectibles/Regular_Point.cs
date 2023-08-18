using GameDevProject_August.Sprites.DNotSentient;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Collectibles
{
    //TODO: Implement the class
    internal class Regular_Point : NotSentient
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
