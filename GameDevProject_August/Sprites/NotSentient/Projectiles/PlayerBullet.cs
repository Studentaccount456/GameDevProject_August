using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameDevProject_August.Sprites.NotSentient.Projectiles
{
    public class PlayerBullet : Projectile , ICloneable
    {
        public float Lifespan = 0f;

        private float _timer;

        private float bulletSpeed = 4f;

        public PlayerBullet(Texture2D texture)
            : base(texture)
        {

        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += facingDirection * bulletSpeed;


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
