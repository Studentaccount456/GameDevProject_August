using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameDevProject_August.Sprites
{
    public class Bullet : Sprite
    {
        private float _timer;

        public Bullet(Texture2D texture) 
            : base(texture)
        {

        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += facingDirection * Speed;


            if (_timer > Lifespan)
            {
                IsRemoved = true;
            }
        }
    }
}
