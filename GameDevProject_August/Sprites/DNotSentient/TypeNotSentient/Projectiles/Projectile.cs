using Microsoft.Xna.Framework.Graphics;

namespace GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles
{
    public class Projectile : NotSentient
    {
        public float Lifespan = 0f;

        protected float _timer;

        public float ProjectileSpeed = 4f;

        public Projectile(Texture2D texture) : base(texture)
        {

        }
    }
}
