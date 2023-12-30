using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles.Bullets.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Player.TypeOfPlayer.ShootingPlayer
{
    public class ShootingPlayer : Player
    {
        public PlayerBullet Bullet;
        protected Vector2 OriginBullet;

        protected bool isShootingCooldown = false;
        protected const float ShootingCooldownDuration = 0.5f;
        protected float shootingCooldownTimer = 0f;

        public ShootingPlayer(Texture2D moveTexture, Texture2D attackTexture, Texture2D idleTexture, Texture2D deathTexture,
                              Texture2D standStillTexture, Texture2D jumpTexture, Texture2D bowDownTexture)
            : base(moveTexture, attackTexture, idleTexture, deathTexture, standStillTexture, jumpTexture, bowDownTexture)
        {
            OriginBullet = new Vector2(30, MoveTexture.Height / 2 - 2);
        }

        //LSP
        public ShootingPlayer()
        {

        }

        protected override void AttackFunctionality(GameTime gameTime, List<Sprite> sprites)
        {
            ShootingFunctionality(gameTime, sprites);
        }


        private void ShootingFunctionality(GameTime gameTime, List<Sprite> sprites)
        {
            ShootCooldown(gameTime);
            if (isAttackingAnimating)
            {
                if (animationShoot.IsAnimationComplete)
                {
                    isAttackingAnimating = false;
                }
            }
            if (Keyboard.GetState().IsKeyDown((Keys)Input.Shoot) && !isShootingCooldown && !isAttackingAnimating)
            {
                Shoot(sprites);
            }
            animationShoot.Update(gameTime);
        }

        private void ShootCooldown(GameTime gameTime)
        {
            if (isShootingCooldown)
            {
                shootingCooldownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (shootingCooldownTimer >= ShootingCooldownDuration)
                {
                    isShootingCooldown = false;
                }
            }
        }

        private void Shoot(List<Sprite> sprites)
        {
            AddBullet(sprites);
            isAttackingAnimating = true;

            isShootingCooldown = true;
            shootingCooldownTimer = 0f;
        }

        private void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as PlayerBullet;
            bullet.facingDirection = facingDirection;
            bullet.Position = Position + OriginBullet;
            bullet.ProjectileSpeed = Speed;
            bullet.Lifespan = 1f;
            bullet.Parent = this;

            sprites.Add(bullet);
        }

    }
}
