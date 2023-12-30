using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles.Bullets.Types
{
    // Class to make distinction between player and enemy bullets (No Friendly fire)
    public class EnemyBullet : Bullet, ICloneable
    {
        public EnemyBullet(Texture2D texture)
            : base(texture)
        {

        }
    }
}
