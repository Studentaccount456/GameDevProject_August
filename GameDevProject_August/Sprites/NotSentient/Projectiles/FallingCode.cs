using GameDevProject_August.Levels;
using GameDevProject_August.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.NotSentient.Projectiles
{
    public class FallingCode : Projectile
    {

        private Texture2D _texture;

        public FallingCode(Texture2D texture)
            : base(texture)
        {
            _texture = texture;
            Position = new Vector2(PlayingState.Random.Next(0, Game1.ScreenWidth - _texture.Width), -_texture.Height);
            Speed = PlayingState.Random.Next(4, 10);
        }

        public override Rectangle RectangleHitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }


        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            Position.Y += Speed;

            // When the bottom of the window is hit
            if (RectangleHitbox.Bottom >= Game1.ScreenHeight)
            {
                IsRemoved = true;
            }
        }

        public void LetCodeFall(List<Sprite> _sprites, Texture2D _texture)
        {
            _sprites.Add(new FallingCode(_texture));
        }
    }
}
