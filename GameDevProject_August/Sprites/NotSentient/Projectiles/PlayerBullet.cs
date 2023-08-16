using GameDevProject_August.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.NotSentient.Projectiles
{
    public class PlayerBullet : Projectile, ICloneable
    {
        public float Lifespan = 0f;

        private float _timer;

        private float bulletSpeed = 4f;


        public override Rectangle RectangleHitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }


        public PlayerBullet(Texture2D texture)
            : base(texture)
        {

        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += facingDirection * bulletSpeed;

            foreach (var block in blocks)
            {
                if(IsTouchingBottomBlock(block) || IsTouchingRightBlock(block) || IsTouchingLeftBlock(block))
                {
                    IsRemoved = true;
                }
            }

            if (_timer > Lifespan)
            {
                IsRemoved = true;
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
