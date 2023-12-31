﻿using GameDevProject_August.Levels;
using GameDevProject_August.Models.Movement;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles.Bullets.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy.AttackingEnemy.SpotterEnemy.ShootingEnemy
{
    public class ShootingEnemy : SpotterEnemy
    {
        public EnemyBullet Bullet;
        protected Vector2 OriginBullet;

        protected float shootDelay;

        public Vector2 EnemyPosition;
        protected bool enemySpotted;

        public ShootingEnemy(Texture2D moveTexture, Texture2D deathTexture, Texture2D attackTexture, Vector2 StartPosition, Vector2 offsetPositionSpotter, int widthSpotter, int heightSpotter) : base(moveTexture, attackTexture, deathTexture, StartPosition, offsetPositionSpotter, widthSpotter, heightSpotter)
        {
            EnemyPosition = new Vector2(0, 0);
            OriginBullet = new Vector2(60, MoveTexture.Height / 2);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            base.Update(gameTime, sprites, blocks);

            ShootingFunctionality(gameTime, sprites);
        }

        protected void ShootingFunctionality(GameTime gameTime, List<Sprite> sprites)
        {
            AttackCooldown(gameTime);
            if (isAttackingAnimating)
            {
                animationAttack.Update(gameTime);
                if (animationAttack.IsAnimationComplete && enemySpotted == false)
                {
                    isAttackingAnimating = false;
                }
            }

            if (!enemySpotted)
            {
                shootDelay = 0f;
            }

            if (enemySpotted && !isAttackCooldown)
            {
                shootDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;
                isAttackingAnimating = true;
                if (EnemyPosition.X > Position.X)
                {
                    Movement.Direction = Direction.Right;
                    facingDirection = Vector2.UnitX;
                }
                else if (EnemyPosition.X < Position.X)
                {
                    Movement.Direction = Direction.Left;
                    facingDirection = -Vector2.UnitX;
                }
                if (shootDelay > 0.75f && !isDeathAnimating)
                {
                    Shoot(sprites);
                }
            }
        }

        protected void Shoot(List<Sprite> sprites)
        {
            AddBullet(sprites);
            isAttackCooldown = true;
            AttackCooldownTimer = 0f;
        }

        protected void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as EnemyBullet;
            bullet.facingDirection = facingDirection;
            bullet.Position = Position + OriginBullet;
            bullet.ProjectileSpeed = Speed;
            bullet.Lifespan = 2f;
            bullet.Parent = this;

            sprites.Add(bullet);
        }

        protected override void UniqueMovingRules(GameTime gameTime, List<Block> blocks)
        {
            throw new NotImplementedException();
        }

        protected override void UniqueCollisionRules(Sprite sprite, Rectangle hitbox, bool isHardSpot)
        {
            throw new NotImplementedException();
        }
    }
}
