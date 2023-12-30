using GameDevProject_August.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles.Bullets
{
    public class Bullet : Projectile
    {
        public Bullet(Texture2D texture) : base(texture)
        {

        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += facingDirection * ProjectileSpeed;

            foreach (var block in blocks)
            {
                if (IsTouchingBottomBlock(block) || IsTouchingRightBlock(block) || IsTouchingLeftBlock(block))
                {
                    IsDestroyed = true;
                }
            }

            if (_timer > Lifespan)
            {
                IsDestroyed = true;
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
