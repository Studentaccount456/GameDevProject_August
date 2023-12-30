using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles.Bullets.Types
{
    public class PlayerBullet : Bullet, ICloneable
    {
        // Class to make distinction between player and enemy bullets (No Friendly fire)
        public PlayerBullet(Texture2D texture)
            : base(texture)
        {

        }
    }
}
