﻿using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Levels;
using GameDevProject_August.Sprites.DNotSentient;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy
{
    public class RatMage : Enemy
    {
        public EnemyBullet Bullet;

        private Animation animationMove;
        private Animation animationIdle;
        private Animation animationShoot;

        public Texture2D ShootTexture;
        public Texture2D IdleTexture;

        private bool isShootingAnimating = false;

        private bool isShootingCooldown = false;
        private const float ShootingCooldownDuration = 1f;
        private float shootingCooldownTimer = 0f;

        private bool isIdling = false;
        private const float IdleTimeoutDuration = 5.0f;
        private float idleTimer = 0f;

        private bool enemySpotted;

        public Rectangle EnemySpotter;

        public Vector2 EnemyPosition;

        private float shootDelay;

        private int _widthSpotter, _heightSpotter;
        private Vector2 _offsetPositonSpotter;

        public RatMage(Texture2D moveTexture, Texture2D shootTexture, Texture2D idleTexture, Texture2D deathTexture,
            Vector2 startPosition, Vector2 offsetPositionSpotter, int widthSpotter, int heightSpotter)
            : base(moveTexture, deathTexture)
        {
            _texture = moveTexture;
            ShootTexture = shootTexture;
            IdleTexture = idleTexture;
            DeathTexture = deathTexture;

            EnemyPosition = new Vector2(0, 0);
            OriginBullet = new Vector2(60, _texture.Height / 2);

            _offsetPositonSpotter = offsetPositionSpotter;
            _widthSpotter = widthSpotter;
            _heightSpotter = heightSpotter;

            Position = startPosition;

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
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            PositionTracker();

            InitializeEnemySpotter(Position, _offsetPositonSpotter, _widthSpotter, _heightSpotter);

            IdleFunctionality(gameTime);

            Move(gameTime, blocks);

            ShootingFunctionality(gameTime, sprites);

            CollisionRules(gameTime, sprites);

            UpdatePositionAndResetVelocity();
        }

        private void CollisionRules(GameTime gameTime, List<Sprite> sprites)
        {
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

            foreach (var sprite in sprites)
            {
                if (sprite is RatMage)
                {
                    continue;
                }

                if (sprite.RectangleHitbox.Intersects(EnemySpotter) && sprite is Archeologist)
                {
                    enemySpotted = true;
                    EnemyPosition.X = sprite.Position.X;
                }
                if (!sprite.RectangleHitbox.Intersects(EnemySpotter) && sprite is Archeologist)
                {
                    enemySpotted = false;
                }

                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is Archeologist && sprite is Sentient sentient)
                {
                    sentient.isDeathAnimating = true;
                }

                GlitchDeathInit(gameTime, sprite, 4);

                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is PlayerBullet && sprite is NotSentient notSentient)
                {
                    Game1.PlayerScore.MainScore++;
                    isDeathAnimating = true;
                    notSentient.IsDestroyed = true;
                }
            }
        }

        private void ShootingFunctionality(GameTime gameTime, List<Sprite> sprites)
        {
            ShootCooldown(gameTime);
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

            if (enemySpotted && !isShootingCooldown)
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
                if (shootDelay > 0.75f && !isDeathAnimating)
                {
                    Shoot(sprites);
                }

            }
        }

        private void Shoot(List<Sprite> sprites)
        {
            AddBullet(sprites);
            isShootingCooldown = true;
            shootingCooldownTimer = 0f;
        }

        private void ShootCooldown(GameTime gameTime)
        {
            // Shooting cooldown
            if (isShootingCooldown)
            {
                shootingCooldownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (shootingCooldownTimer >= ShootingCooldownDuration)
                {
                    isShootingCooldown = false;
                }
            }
        }

        private void IdleFunctionality(GameTime gameTime)
        {
            idleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (idleTimer >= IdleTimeoutDuration + 1f && !isShootingAnimating)
            {
                isIdling = false;
                idleTimer = 0;
            }
            else if (idleTimer >= IdleTimeoutDuration && !isShootingAnimating)
            {
                isIdling = true;
                animationIdle.Update(gameTime);
            }
        }

        private void InitializeEnemySpotter(Vector2 position, Vector2 offsetPositionSpotter, int widthSpotter, int heightSpotter)
        {
            // Initialize in Constructor + use RemoveEnemySpotterSpotted() when spotter needs dissapear after spot
            EnemySpotter = new Rectangle((int)(position.X - offsetPositionSpotter.X), (int)(position.Y - offsetPositionSpotter.Y), widthSpotter, heightSpotter);
            // Otherwise put in update so the Position updates so the spot can be reset
        }

        private void PositionTracker()
        {
            PositionXRectangleHitbox = (int)Position.X;
            PositionYRectangleHitbox = (int)Position.Y;
        }

        private void UpdatePositionAndResetVelocity()
        {
            Position += Velocity;

            Velocity = Vector2.Zero;
        }

        private void Move(GameTime gameTime, List<Block> blocks)
        {
            if (!isDeathAnimating && !isIdling && !isShootingAnimating)
            {
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
                animationMove.Update(gameTime);
            }
        }

        private void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as EnemyBullet;
            bullet.facingDirection = facingDirection;
            bullet.Position = Position + OriginBullet;
            bullet.BulletSpeed = Speed;
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
