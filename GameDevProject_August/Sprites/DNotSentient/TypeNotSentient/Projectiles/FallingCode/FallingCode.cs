using GameDevProject_August.Levels;
using GameDevProject_August.States.StateTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles.FallingCode
{
    public class FallingCode : Projectile
    {
        public FallingCode(Texture2D texture)
            : base(texture)
        {
            Position = new Vector2(PlayingState.Random.Next(0, Game1.ScreenWidth - staticTexture.Width), -staticTexture.Height);
            ProjectileSpeed = PlayingState.Random.Next(4, 10);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            Position.Y += ProjectileSpeed;

            // When the bottom of the window is hit
            if (RectangleHitbox.Bottom >= Game1.ScreenHeight)
            {
                IsDestroyed = true;
            }
        }

        public void LetCodeFall(List<Sprite> _sprites, Texture2D _texture)
        {
            _sprites.Add(new FallingCode(_texture));
        }
    }
}
