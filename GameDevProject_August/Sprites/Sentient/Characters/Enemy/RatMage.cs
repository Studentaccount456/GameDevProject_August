﻿using GameDevProject_August.AnimationClasses;
using GameDevProject_August.AnimationClasses.AnimationMethods;
using GameDevProject_August.Levels;
using GameDevProject_August.Models;
using GameDevProject_August.Sprites.NotSentient.Collectibles;
using GameDevProject_August.Sprites.NotSentient.Projectiles;
using GameDevProject_August.Sprites.Sentient.Characters.Main;
using GameDevProject_August.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject_August.Sprites.Sentient.Characters.Enemy
{
    public class RatMage : Sprite
    {
        public EnemyBullet Bullet;

        public bool HasDied = false;

        private Animation animationMove;
        private Animation animationDeath;
        private Animation animationIdle;
        private Animation animationShoot;

        public Texture2D ShootTexture;
        public Texture2D IdleTexture;
        public Texture2D DeathTexture;

        private bool isDeathAnimating = false;
        private bool isShootingAnimating = false;

        private bool isShootingCooldown = false;
        private const float ShootingCooldownDuration = 1f;
        private float shootingCooldownTimer = 0f;

        private bool isIdling = false;
        private const float IdleTimeoutDuration = 5.0f;
        private float idleTimer = 0f;
        private bool standStillNoIdle = false;


        private int deathAnimationFrameIndex = 0;

        private bool reachedFourthDeathFrame = false;

        public Rectangle DeathRectangle;

        private bool enemySpotted;

        public Rectangle EnemySpotter;

        public Vector2 EnemyPosition;

        private float shootDelay;

        public RatMage(Texture2D moveTexture, Texture2D shootTexture, Texture2D idleTexture, Texture2D deathTexture)
            : base(moveTexture)
        {
            _texture = moveTexture;
            ShootTexture = shootTexture;
            IdleTexture = idleTexture;
            DeathTexture = deathTexture;

            EnemyPosition = new Vector2(0, 0);
            OriginBullet = new Vector2(60, _texture.Height / 2);

            // Standard walks right
            #region MoveAnimation
            animationMove = new Animation(AnimationType.Move, moveTexture);
            animationMove.AddFrame(new AnimationFrame(new Rectangle(0, 0, 60, 57)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(96, 0, 57, 57)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(192, 0, 54, 57)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(288, 0, 57, 57)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(384, 0, 60, 57)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(480, 0, 60, 57)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(579, 0, 57, 57)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(672, 0, 60, 57)));
            #endregion

            //Height is 48 for each frame
            #region animationCast
            animationShoot = new Animation(AnimationType.Attack, shootTexture);
            animationShoot.fps = 6;
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(0, 0, 60, 48)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(96, 0, 51, 48)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(192, 0, 42, 48)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(288, 0, 42, 48)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(384, 0, 72, 48)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(480, 0, 69, 48)));
            #endregion

            //Height is 44 for each frame
            #region Idle
            animationIdle = new Animation(AnimationType.Idle, idleTexture);
            animationIdle.fps = 10;
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(0, 0, 60, 48)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(99, 0, 57, 48)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(195, 0, 57, 48)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(288, 0, 60, 48)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(384, 0, 60, 48)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(483, 0, 57, 48)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(579, 0, 57, 48)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(672, 0, 60, 48)));
            #endregion


            #region Death
            animationDeath = new Animation(AnimationType.Death, deathTexture);
            animationDeath.fps = 4;
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(0, 0, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(64, 0, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(128, 0, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(192, 0, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(0, 64, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(64, 64, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(128, 64, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(192, 64, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(0, 128, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(64, 128, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(128, 128, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(192, 128, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(0, 192, 64, 64)));
            #endregion
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            PositionXRectangleHitbox = (int)Position.X;
            PositionYRectangleHitbox = (int)Position.Y;
            EnemySpotter = new Rectangle((int)Position.X - 300, (int)Position.Y - 71, 600, 120);


            idleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (idleTimer >= IdleTimeoutDuration + 1f && !isShootingAnimating)
            {
                standStillNoIdle = true;
                isIdling = false;
                idleTimer = 0;
            }
            else if (idleTimer >= IdleTimeoutDuration && !isShootingAnimating)
            {
                isIdling = true;
                standStillNoIdle = false;
                animationIdle.Update(gameTime);
            }

            if (!isDeathAnimating && !isIdling && !isShootingAnimating)
            {
                Move();
                animationMove.Update(gameTime);
            }

            if (facingDirectionIndicator || !facingDirectionIndicator && !isShootingAnimating)
            {
                WidthRectangleHitbox = 60;
                HeightRectangleHitbox = 49;
                PositionYRectangleHitbox = (int)Position.Y;
            }
            else
            {
                WidthRectangleHitbox = 60;
                HeightRectangleHitbox = 49;
            }

            // Shooting cooldown
            if (isShootingCooldown)
            {
                shootingCooldownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (shootingCooldownTimer >= ShootingCooldownDuration)
                {
                    isShootingCooldown = false;
                }
            }

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

            if (enemySpotted && !isShootingCooldown && !isShootingCooldown)
            {
                shootDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;
                isShootingAnimating = true;
                if (EnemyPosition.X > Position.X)
                {
                    facingDirectionIndicator = true;
                    facingDirection = Vector2.UnitX;
                }
                else if (EnemyPosition.X < Position.X)
                {
                    facingDirectionIndicator = false;
                    facingDirection = -Vector2.UnitX;
                }
                if (shootDelay > 0.75f)
                {
                    AddBullet(sprites);
                    isShootingCooldown = true;
                    shootingCooldownTimer = 0f;
                }

            }


            foreach (var sprite in sprites)
            {
                if (sprite is RatMage)
                {
                    continue;
                }

                if (sprite.RectangleHitbox.Intersects(EnemySpotter) && sprite is MainCharacter)
                {
                    enemySpotted = true;
                    EnemyPosition.X = sprite.Position.X;
                }
                if (!sprite.RectangleHitbox.Intersects(EnemySpotter) && sprite is MainCharacter)
                {
                    enemySpotted = false;
                }

                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is PlayerBullet)
                {
                    Game1.PlayerScore.MainScore++;
                    HasDied = true;
                    isDeathAnimating = true;
                    sprite.IsRemoved = true;
                }

                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is MainCharacter)
                {
                    HasDied = true;
                    sprite.isDeathAnimating = true;
                }

                if (isDeathAnimating)
                {
                    DeathRectangle = new Rectangle((int)Position.X, (int)Position.Y, 64, 64);

                    animationDeath.Update(gameTime);

                    deathAnimationFrameIndex = animationDeath.CurrentFrameIndex;

                    if (deathAnimationFrameIndex == 3) // 4th frame
                    {
                        reachedFourthDeathFrame = true;
                    }

                    if (reachedFourthDeathFrame && animationDeath.IsAnimationComplete)
                    {
                        PieceOfCodeToFall = 4;
                        IsRemoved = true;
                    }

                    if (sprite.RectangleHitbox.Intersects(DeathRectangle) && sprite is MainCharacter)
                    {
                        sprite.isDeathAnimating = true;
                    }

                    if (deathAnimationFrameIndex > 6)
                    {
                        DeathRectangle.Width = 0;
                        DeathRectangle.Height = 0;
                    }

                }
            }

            foreach (var block in blocks)
            {
                if (IsTouchingLeftBlock(block) && block.EnemyBehavior_2 == true)
                {
                    facingDirectionIndicator = false;
                }
                else if (IsTouchingRightBlock(block) && block.EnemyBehavior_2 == true)
                {
                    facingDirectionIndicator = true;
                }
            }

            Position += Velocity;

            Velocity = Vector2.Zero;

            if (isDeathAnimating == true)
            {
                animationDeath.Update(gameTime);
            }
        }

        private void Move()
        {
            if (!facingDirectionIndicator)
            {
                Velocity.X -= Speed;
                facingDirection = -Vector2.UnitX;
            }
            if (facingDirectionIndicator)
            {
                Velocity.X += Speed;
                facingDirection = Vector2.UnitX;
            }

            Position = Vector2.Clamp(Position, new Vector2(0 - RectangleHitbox.Width, 0 + RectangleHitbox.Height / 2), new Vector2(Game1.ScreenWidth - RectangleHitbox.Width, Game1.ScreenHeight - RectangleHitbox.Height / 2));
        }

        private void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as EnemyBullet;
            bullet.facingDirection = facingDirection;
            bullet.Position = Position + OriginBullet;
            bullet.Speed = Speed;
            bullet.Lifespan = 1f;
            bullet.Parent = this;

            sprites.Add(bullet);
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDeathAnimating)
            {
                if (reachedFourthDeathFrame)
                {
                    spriteBatch.Draw(DeathTexture, Position, animationDeath.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(DeathTexture, Position, animationDeath.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
                    if (animationDeath.IsAnimationComplete)
                    {
                        IsRemoved = true;
                    }
                }
            }
            else if (isShootingAnimating)
            {
                if (facingDirectionIndicator == true)
                {
                    spriteBatch.Draw(ShootTexture, Position, animationShoot.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
                }
                else if (facingDirectionIndicator == false)
                {
                    spriteBatch.Draw(ShootTexture, Position, animationShoot.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
                }
            }
            else if (isIdling)
            {
                if (facingDirectionIndicator == true)
                {
                    spriteBatch.Draw(IdleTexture, Position, animationIdle.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
                }
                else if (facingDirectionIndicator == false)
                {
                    spriteBatch.Draw(IdleTexture, Position, animationIdle.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
                }
            }
            else if (facingDirectionIndicator == true && !isIdling)
            {
                spriteBatch.Draw(_texture, Position + new Vector2(0, -4), animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (facingDirectionIndicator == false && !isIdling)
            {
                spriteBatch.Draw(_texture, Position + new Vector2(0, -4), animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }

            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
            spriteBatch.DrawRectangle(DeathRectangle, Color.Red);
            spriteBatch.DrawRectangle(EnemySpotter, Color.Red);
        }
        
    }
}
