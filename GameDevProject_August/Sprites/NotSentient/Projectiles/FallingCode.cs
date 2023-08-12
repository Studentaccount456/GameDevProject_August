using GameDevProject_August.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.NotSentient.Projectiles
{
    public class FallingCode : Projectile
    {
        public FallingCode(Texture2D texture)
            : base(texture)
        {
            Position = new Vector2(GameState.Random.Next(0, Game1.ScreenWidth - _texture.Width), -_texture.Height);
            Speed = GameState.Random.Next(3, 10);
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
