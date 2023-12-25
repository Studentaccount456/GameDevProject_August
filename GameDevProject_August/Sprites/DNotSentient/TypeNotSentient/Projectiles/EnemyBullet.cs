using GameDevProject_August.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles
{
    public class EnemyBullet : Projectile, ICloneable
    {
        public float Lifespan = 0f;

        private float _timer;

        public float BulletSpeed = 4f;

        public EnemyBullet(Texture2D texture)
            : base(texture)
        {

        }

        public override Rectangle RectangleHitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, MoveTexture.Width, MoveTexture.Height);
            }
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += facingDirection * BulletSpeed;


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
