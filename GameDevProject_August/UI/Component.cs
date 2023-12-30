using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDevProject_August.UI
{
    public abstract class Component
    {
        public abstract void Draw(GameTime gametime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gametime);
    }
}
