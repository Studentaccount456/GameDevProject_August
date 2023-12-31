﻿using GameDevProject_August.AnimationClasses;
using GameDevProject_August.AnimationClasses.AnimationMethods;
using GameDevProject_August.Levels;
using GameDevProject_August.Models.Movement;
using GameDevProject_August.Sprites.DNotSentient;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles.Bullets.Types;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player.TypeOfPlayer.ShootingPlayer.Characters;
using GameDevProject_August.States.StateTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy
{
    abstract public class Enemy : Sentient
    {
        protected Animation animationMove;

        protected Uniaxial_Movement Movement = new Uniaxial_Movement()
        {
            Direction = Direction.Right
        };

        protected Texture2D DeathTexture;

        protected Animation animationDeath;

        public Rectangle DeathRectangle;

        protected int deathAnimationFrameIndex = 0;

        protected bool reachedFourthDeathFrame = false;

        protected Dictionary<string, Rectangle> hitboxes = new Dictionary<string, Rectangle>();

        public Rectangle AdditionalHitBox_1;

        protected Animation animationIdle;

        public Texture2D IdleTexture;

        protected bool isIdling = false;

        protected const float IdleTimeoutDuration = 5.0f;

        protected float idleTimer = 0f;

        protected AnimationHandler animationHandlerEnemy;

        protected int numberOfCodeToFall = 0;

        public Enemy(Texture2D moveTexture, Texture2D deathTexture, Vector2 StartPosition) : base(moveTexture)
        {
            DeathTexture = deathTexture;

            animationHandlerEnemy = new AnimationHandler();

            //Height is 44 for each frame
            #region Death
            animationDeath = new Animation(AnimationType.Death, deathTexture);
            animationDeath.fps = 4;
            animationDeath.AddConsistentFrames(64, 0, 64, 64, 4);
            animationDeath.AddConsistentFramesWithStartCoördinates(0, 64, 64, 0, 64, 64, 4);
            animationDeath.AddConsistentFramesWithStartCoördinates(0, 128, 64, 0, 64, 64, 4);
            animationDeath.AddFrame(0, 192, 64, 64);
            #endregion

            animationMove = new Animation(AnimationType.Move, moveTexture);
            animationMove.fps = 8;

            Position = StartPosition;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            HitBoxTracker();

            Move(gameTime, blocks);

            CollisionRules(gameTime, sprites);

            IdleFunctionality(gameTime);
        }

        protected void Move(GameTime gameTime, List<Block> blocks)
        {
            if (!isDeathAnimating)
            {
                UniqueMovingRules(gameTime, blocks);
            }
            animationMove.Update(gameTime);

            Position = Vector2.Clamp(Position, new Vector2(0 - RectangleHitbox.Width, 0 + RectangleHitbox.Height / 2), new Vector2(Game1.ScreenWidth - RectangleHitbox.Width, Game1.ScreenHeight - RectangleHitbox.Height / 2));
        }

        protected abstract void UniqueMovingRules(GameTime gameTime, List<Block> blocks);

        protected void UpdatePositionAndResetVelocity()
        {
            Position += Velocity;

            Velocity = Vector2.Zero;
        }

        protected void CollisionRules(GameTime gameTime, List<Sprite> sprites)
        {
            foreach (var sprite in sprites)
            {
                foreach (var hitbox in hitboxes)
                {
                    bool IsHardSpot = false;
                    if (hitbox.Key.StartsWith("HardSpot"))
                    {
                        IsHardSpot = true;
                    }
                    {
                        UniqueCollisionRules(sprite, hitbox.Value, IsHardSpot);

                        if (sprite.RectangleHitbox.Intersects(hitbox.Value) && sprite is PlayerBullet && sprite is NotSentient notSentient && hitbox.Key.StartsWith("SoftSpot"))
                        {
                            Game1.PlayerScore.MainScore++;
                            isDeathAnimating = true;
                            notSentient.IsDestroyed = true;
                        }
                        if (sprite.RectangleHitbox.Intersects(hitbox.Value) && sprite is Archeologist archeologist)
                        {
                            archeologist.isDeathAnimating = true;
                        }
                    }
                }

                GlitchDeathInit(gameTime, sprite, numberOfCodeToFall);

                UpdatePositionAndResetVelocity();
            }
        }

        protected abstract void UniqueCollisionRules(Sprite sprite, Rectangle hitbox, bool isHardSpot);

        abstract protected void HitBoxTracker();

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDeathAnimating)
            {
                animationHandlerEnemy.DrawAnimation(spriteBatch, animationDeath, Position, Direction.Right);
            }
            else if (isIdling)
            {
                animationHandlerEnemy.DrawAnimation(spriteBatch, animationIdle, Position, Movement.Direction);
            }
            else
            {
                animationHandlerEnemy.DrawAnimation(spriteBatch, animationMove, Position + offsetMoveAnimation, Movement.Direction);
            }
        }

        protected void GlitchDeathInit(GameTime gameTime, Sprite sprite, int pieceOfCodeToFall)
        {
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
                    PlayingState.whichCodeFalls = pieceOfCodeToFall;
                    IsKilled = true;
                }

                if (sprite.RectangleHitbox.Intersects(DeathRectangle) && sprite is Archeologist && sprite is Sentient sentient)
                {
                    sentient.isDeathAnimating = true;
                }

                WidthRectangleHitbox = 0;
                HeightRectangleHitbox = 0;

                if (deathAnimationFrameIndex > 6)
                {
                    DeathRectangle.Width = 0;
                    DeathRectangle.Height = 0;
                }

            }
        }

        protected virtual void IdleFunctionality(GameTime gameTime)
        {

        }
    }


}
