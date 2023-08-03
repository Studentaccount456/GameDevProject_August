using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject_August.Sprites
{
    public class FallingCode : Sprite
    {
        public FallingCode(Texture2D texture) 
            : base(texture)
        {
            Position = new Vector2(Game1.Random.Next(0, Game1.ScreenWidth - _texture.Width), - _texture.Height);
            Speed = Game1.Random.Next(3, 10);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Position.Y += Speed;
            
            // When the bottom of the window is hit
            if (Rectangle.Bottom >= Game1.ScreenHeight)
            {
                IsRemoved = true;
            }
        }
    }
}
