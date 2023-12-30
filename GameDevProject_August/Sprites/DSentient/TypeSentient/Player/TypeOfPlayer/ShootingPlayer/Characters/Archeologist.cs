using GameDevProject_August.AnimationClasses;
using GameDevProject_August.AnimationClasses.AnimationMethods;
using GameDevProject_August.Levels;
using GameDevProject_August.Levels.BlockTypes;
using GameDevProject_August.Models;
using GameDevProject_August.Models.Movement;
using GameDevProject_August.Sprites.DNotSentient;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Collectibles;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles.Bullets.Types;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles.FallingCode;
using GameDevProject_August.States;
using GameDevProject_August.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Player.TypeOfPlayer.ShootingPlayer.Characters
{
    public class Archeologist : ShootingPlayer
    {
        public PlayerBullet Bullet;
        private Vector2 OriginBullet;

        protected bool isCharacterMoving;

        protected Biaxial_Movement Movement = new Biaxial_Movement()
        {
            Direction_X = Direction_X.Right,
            Direction_Y = Direction_Y.None
        };

        // Consistent Hitbox
        public override Rectangle RectangleHitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 34, 44);
            }
        }


        public Archeologist(Texture2D moveTexture, Texture2D shootTexture, Texture2D idleTexture, Texture2D deathTexture, Texture2D standStillTexture, Texture2D jumpTexture, Texture2D bowDownTexture)
            : base(moveTexture)
        {
            AnimationHandler_Player = new AnimationHandler();

            StandStillTexture = standStillTexture;

            JumpTexture = jumpTexture;

            BowDownTexture = bowDownTexture;

            hasJumped = true;

            OriginBullet = new Vector2(30, MoveTexture.Height / 2 - 2);



            // Standard walks right
            #region MoveAnimation
            animationMove = new Animation(AnimationType.Move, moveTexture);
            animationMove.AddFrame(new AnimationFrame(new Rectangle(0, 0, 30, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(128, 0, 28, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(256, 0, 28, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(384, 0, 28, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(512, 0, 30, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(640, 0, 34, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(768, 0, 34, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(896, 0, 34, 50)));
            #endregion

            //Height is 48 for each frame
            #region animationShoot
            animationShoot = new Animation(AnimationType.Attack, shootTexture);
            animationShoot.fps = 4;
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(0, 0, 36, 50)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(126, 0, 44, 50)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(256, 0, 40, 50)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(384, 0, 44, 50)));
            #endregion

            //Height is 44 for each frame
            #region Idle
            animationIdle = new Animation(AnimationType.Idle, idleTexture);
            animationIdle.fps = 8;
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(0, 0, 32, 44)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(128, 0, 32, 44)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(256, 0, 32, 44)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(384, 0, 32, 44)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(512, 0, 32, 44)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(640, 0, 32, 44)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(768, 0, 32, 44)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(896, 0, 32, 44)));
            #endregion

            //Height is 44 for each frame
            #region DeathAnimation
            animationDeath = new Animation(AnimationType.Death, deathTexture);
            animationDeath.fps = 8;
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(0, 0, 32, 44)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(128, 0, 42, 44)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(260, 0, 50, 44)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(388, 0, 50, 44)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(516, 0, 50, 44)));
            #endregion

            #region JumpAnimation
            animationJump = new Animation(AnimationType.Jump, jumpTexture);
            animationJump.AddFrame(new AnimationFrame(new Rectangle(0, 0, 30, 44)));
            animationJump.AddFrame(new AnimationFrame(new Rectangle(128, 0, 28, 44)));
            animationJump.AddFrame(new AnimationFrame(new Rectangle(256, 0, 28, 44)));
            animationJump.AddFrame(new AnimationFrame(new Rectangle(384, 0, 28, 44)));
            animationJump.AddFrame(new AnimationFrame(new Rectangle(510, 0, 28, 44)));
            animationJump.AddFrame(new AnimationFrame(new Rectangle(640, 0, 28, 44)));
            animationJump.AddFrame(new AnimationFrame(new Rectangle(768, 0, 28, 44)));
            animationJump.AddFrame(new AnimationFrame(new Rectangle(896, 0, 30, 44)));
            #endregion

            animationDictionary = new Dictionary<string, Animation>
{
                                { "MoveAnimation", animationMove },
                                { "AttackAnimation", animationShoot },
                                { "DeathAnimation", animationDeath },
                                { "IdleAnimation", animationIdle },
                                { "JumpAnimation", animationJump }
                                };
        }



        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            //_previousKey = _currentKey;
            //_currentKey = Keyboard.GetState();

            ImplementGravity(gameTime);

            //Moving (Horizontal, Vertical (Including Jump)
            Move(gameTime, blocks);

            CollisionRules(sprites, blocks);

            IdleFunctionality(gameTime);

            ShootingFunctionality(gameTime, sprites);

            DeathTriggered(gameTime);

            UpdatePositionAndResetVelocity();
        }

        private void ImplementGravity(GameTime gameTime)
        {
            Velocity.Y += GravityAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void UpdatePositionAndResetVelocity()
        {
            Position += Velocity;

            if (!hasJumped)
            {
                Velocity = Vector2.Zero;
            }
        }

        private void DeathTriggered(GameTime gameTime)
        {
            if (isDeathAnimating == true)
            {
                animationDeath.Update(gameTime);
            }
        }

        private void CollisionRules(List<Sprite> sprites, List<Block> blocks)
        {
            foreach (var sprite in sprites)
            {
                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && (sprite is FallingCode || sprite is EnemyBullet) && sprite is NotSentient notSentient)
                {
                    isDeathAnimating = true;
                    notSentient.IsDestroyed = true;
                }

                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is Regular_Point && sprite is NotSentient notSentient2)
                {
                    Score.MainScore++;
                    PlayingState.whichCodeFalls = 5;
                    notSentient2.IsDestroyed = true;
                }
            }

            foreach (var block in blocks)
            {
                if (block is Block && block is not ThreePointsType && block is not SevenPointsType && block is not InvisibleBlock)
                {
                    if ((Velocity.X > 0 && IsTouchingLeftBlock(block) ||
                        Velocity.X < 0 && IsTouchingRightBlock(block)) && !hasJumped)
                    {
                        Velocity.X = 0;
                    }

                    if (Velocity.Y > 0 && IsTouchingTopBlock(block) && !hasJumped ||
                        Velocity.Y < 0 && IsTouchingBottomBlock(block))
                    {
                        Velocity.Y = 0;
                    }

                    if (IsTouchingTopBlock(block))
                    {
                        hasJumped = false;
                    }
                }
                else if (block is ThreePointsType)
                {
                    if (block.BlockRectangle.Intersects(RectangleHitbox) && Game1.PlayerScore.MainScore >= 3)
                    {
                        PlayingState.isNextLevelTrigger = true;
                    }
                }
                else if (block is SevenPointsType)
                {
                    if (block.BlockRectangle.Intersects(RectangleHitbox) && Game1.PlayerScore.MainScore >= 7)
                    {
                        PlayingState.isNextLevelTrigger = true;
                    }
                }
            }
        }

        private void Shoot(List<Sprite> sprites)
        {
            AddBullet(sprites);
            isShootingAnimating = true;

            isShootingCooldown = true;
            shootingCooldownTimer = 0f;
        }

        private void ShootingFunctionality(GameTime gameTime, List<Sprite> sprites)
        {
            ShootCooldown(gameTime);
            if (isShootingAnimating)
            {
                if (animationShoot.IsAnimationComplete)
                {
                    isShootingAnimating = false;
                }
            }
            if (Keyboard.GetState().IsKeyDown((Keys)Input.Shoot) && !isShootingCooldown && !isShootingAnimating)
            {
                Shoot(sprites);
            }
            animationShoot.Update(gameTime);
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
            keyPressed = Keyboard.GetState().IsKeyDown((Keys)Input.Left) || Keyboard.GetState().IsKeyDown((Keys)Input.Right) ||
                  Keyboard.GetState().IsKeyDown((Keys)Input.Up) || Keyboard.GetState().IsKeyDown((Keys)Input.Down) ||
                  Keyboard.GetState().IsKeyDown((Keys)Input.Shoot) && isShootingAnimating;

            if (keyPressed)
            {
                idleTimer = 0f;
                isIdling = false;
                standStillNoIdle = false;

            }
            else
            {
                idleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (idleTimer >= IdleTimeoutDuration)
                {
                    isIdling = true;
                    standStillNoIdle = false;
                    animationIdle.Update(gameTime);
                }
                else
                {
                    standStillNoIdle = true;
                }
            }
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

        private void Move(GameTime gameTime, List<Block> blocks)
        {
            if (!isShootingAnimating)
            {
                if (Input == null)
                    return;

                if (!Keyboard.GetState().IsKeyDown((Keys)Input.Left) && !Keyboard.GetState().IsKeyDown((Keys)Input.Right) && !Keyboard.GetState().IsKeyDown((Keys)Input.Down) && !Keyboard.GetState().IsKeyDown((Keys)Input.Up))
                {
                    isCharacterMoving = false;
                    Movement.Direction_Y = Direction_Y.None;
                } else if (isDeathAnimating)
                {
                     Velocity = Vector2.Zero;
                }
                else
                {
                    isCharacterMoving = true;

                    if (Keyboard.GetState().IsKeyDown((Keys)Input.Up))
                    {
                        Movement.Direction_Y = Direction_Y.Up;
                    }

                    if (Keyboard.GetState().IsKeyDown((Keys)Input.Up) && !hasJumped)
                    {
                        hasJumped = true;
                        Movement.Direction_Y = Direction_Y.Up;

                        if (hasJumped)
                        {
                            Velocity.Y = -20f;
                            if (Keyboard.GetState().IsKeyDown((Keys)Input.Right))
                            {
                                Velocity.X = Speed;
                            }
                            if (Keyboard.GetState().IsKeyDown((Keys)Input.Left))
                            {
                                Velocity.X = -Speed;
                            }
                        }
                    }
                    if (hasJumped)
                    {
                        foreach (var block in blocks)
                        {
                            if (IsTouchingTopBlock(block))
                            {
                                hasJumped = true;
                            }
                        }
                    }
                    if (Keyboard.GetState().IsKeyDown((Keys)Input.Down))
                    {
                        Movement.Direction_Y = Direction_Y.Down;
                    }
                    if (Keyboard.GetState().IsKeyDown((Keys)Input.Left) && !hasJumped)
                    {
                        Velocity.X -= Speed;
                        facingDirection = -Vector2.UnitX;
                        Movement.Direction_X = Direction_X.Left;
                    }
                    if (Keyboard.GetState().IsKeyDown((Keys)Input.Right) && !hasJumped)
                    {
                        Velocity.X += Speed;
                        facingDirection = Vector2.UnitX;
                        Movement.Direction_X = Direction_X.Right;

                    }
                }
            }
            Position = Vector2.Clamp(Position, new Vector2(0, 0 + RectangleHitbox.Height / 4), new Vector2(Game1.ScreenWidth - RectangleHitbox.Width, Game1.ScreenHeight - RectangleHitbox.Height));

            animationMove.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDeathAnimating)
            {
                if (Movement.Direction_X == Direction_X.Left)
                {
                    AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["DeathAnimation"], Position, Direction.Left);
                }
                else if (Movement.Direction_X == Direction_X.Right)
                {
                    AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["DeathAnimation"], Position, Direction.Right);
                }
                if (animationDictionary["DeathAnimation"].IsAnimationComplete)
                {
                    HasDied = true;
                    IsKilled = true;
                }
            }
            else if (isShootingAnimating)
            {
                if (Movement.Direction_X == Direction_X.Right)
                {
                    AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["AttackAnimation"], Position, Direction.Right);
                }
                else if (Movement.Direction_X == Direction_X.Left)
                {
                    AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["AttackAnimation"], Position, Direction.Left);
                }
                if (animationDictionary["AttackAnimation"].IsAnimationComplete)
                {
                    isShootingAnimating = false;
                }
            }
            else if (hasJumped && Movement.Direction_X == Direction_X.Right && Movement.Direction_Y == Direction_Y.None)
            {
                AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["JumpAnimation"], Position, Direction.Right);
            }
            else if (hasJumped && Movement.Direction_X == Direction_X.Left && Movement.Direction_Y == Direction_Y.None)
            {
                AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["JumpAnimation"], Position, Direction.Left);
            }
            else if (Movement.Direction_X == Direction_X.Left && isCharacterMoving == true)
            {
                AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["MoveAnimation"], Position, Direction.Left);
            }
            else if (Movement.Direction_X == Direction_X.Right && isCharacterMoving == true)
            {
                AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["MoveAnimation"], Position, Direction.Right);
            }
            else if (isIdling)
            {
                if (Movement.Direction_X == Direction_X.Right && isCharacterMoving == false)
                {
                    AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["IdleAnimation"], Position, Direction.Right);
                }
                else if (Movement.Direction_X == Direction_X.Left && isCharacterMoving == false)
                {
                    AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["IdleAnimation"], Position, Direction.Left);
                }
            }

            else if (Movement.Direction_X == Direction_X.Right && standStillNoIdle == true && !isShootingAnimating || Movement.Direction_X == Direction_X.Right && !isShootingAnimating && Movement.Direction_Y == Direction_Y.None || Movement.Direction_X == Direction_X.Right && !isShootingAnimating && Movement.Direction_Y == Direction_Y.Up)
            {
                AnimationHandler_Player.DrawOneFrameAnimation(spriteBatch, StandStillTexture, Position, Direction.Right);
            }
            else if (Movement.Direction_X == Direction_X.Left && standStillNoIdle == true && !isShootingAnimating || Movement.Direction_X == Direction_X.Left && !isShootingAnimating && Movement.Direction_Y == Direction_Y.Up || Movement.Direction_X == Direction_X.Left && !isShootingAnimating && Movement.Direction_Y == Direction_Y.None)
            {
                AnimationHandler_Player.DrawOneFrameAnimation(spriteBatch, StandStillTexture, Position, Direction.Left);
            }
            else if (Movement.Direction_X == Direction_X.Right && Movement.Direction_Y == Direction_Y.Down && !hasJumped)
            {
                AnimationHandler_Player.DrawOneFrameAnimation(spriteBatch, BowDownTexture, Position + new Vector2(0, 2), Direction.Right);
            }
            else if (Movement.Direction_X == Direction_X.Left && Movement.Direction_Y == Direction_Y.Down && !hasJumped)
            {
                AnimationHandler_Player.DrawOneFrameAnimation(spriteBatch, BowDownTexture, Position + new Vector2(0, 2), Direction.Left);
            }
            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);

        }
    }
}