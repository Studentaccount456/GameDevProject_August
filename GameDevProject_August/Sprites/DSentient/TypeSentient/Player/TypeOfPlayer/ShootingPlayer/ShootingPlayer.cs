using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Player.TypeOfPlayer.ShootingPlayer
{
    public class ShootingPlayer : Player
    {
        protected bool isShootingAnimating = false;

        protected bool isShootingCooldown = false;
        protected const float ShootingCooldownDuration = 0.5f;
        protected float shootingCooldownTimer = 0f;

        public ShootingPlayer(Texture2D texture) : base(texture)
        {
        }
    }
}
