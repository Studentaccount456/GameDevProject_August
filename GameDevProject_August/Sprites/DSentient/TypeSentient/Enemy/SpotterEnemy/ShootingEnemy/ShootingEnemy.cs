using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Levels;
using GameDevProject_August.Models.Movement;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy.SpotterEnemy.ShootingEnemy
{
    public class ShootingEnemy : SpotterEnemy
    {
        public EnemyBullet Bullet;
        public Texture2D ShootTexture;
        protected bool isShootingAnimating = false;

        protected bool enemySpotted;

        public Vector2 EnemyPosition;

        protected float shootDelay;

        protected Vector2 OriginBullet;

        protected Animation animationShoot;




        public ShootingEnemy(Texture2D moveTexture, Texture2D deathTexture, Texture2D shootTexture, Vector2 StartPosition, Vector2 offsetPositionSpotter, int widthSpotter, int heightSpotter) : base(moveTexture, deathTexture, StartPosition, offsetPositionSpotter, widthSpotter, heightSpotter)
        {
            ShootTexture = shootTexture;

            EnemyPosition = new Vector2(0, 0);
            OriginBullet = new Vector2(60, MoveTexture.Height / 2);
            animationShoot = new Animation(AnimationType.Attack, shootTexture);

        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            base.Update(gameTime, sprites, blocks);

            ShootingFunctionality(gameTime, sprites);
        }

        protected override void UniqueCollisionRules(Sprite sprite, Rectangle hitbox, bool isHardSpot)
        {
            throw new NotImplementedException();
        }

        protected override void UniqueDrawRules(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        protected override void UniqueMovingRules(GameTime gameTime, List<Block> blocks)
        {
            throw new NotImplementedException();
        }

        protected void ShootingFunctionality(GameTime gameTime, List<Sprite> sprites)
        {
            AttackCooldown(gameTime);
            if (isShootingAnimating)
            {
                animationShoot.Update(gameTime);
                if (animationShoot.IsAnimationComplete && enemySpotted == false)
                {
                    isShootingAnimating = false;
                }
            }

            if (!enemySpotted)
            {
                shootDelay = 0f;
            }

            if (enemySpotted && !isAttackCooldown)
            {
                shootDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;
                isShootingAnimating = true;
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
            bullet.BulletSpeed = Speed;
            bullet.Lifespan = 1f;
            bullet.Parent = this;

            sprites.Add(bullet);
        }
    }
}
