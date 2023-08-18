using GameDevProject_August.AnimationClasses;
using GameDevProject_August.AnimationClasses.AnimationMethods;
using GameDevProject_August.Levels;
using GameDevProject_August.Levels.BlockTypes;
using GameDevProject_August.Models;
using GameDevProject_August.Sprites.DNotSentient;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Collectibles;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles;
using GameDevProject_August.States;
using GameDevProject_August.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Player.Characters
{
    public class Archeologist : Sentient
    {
        public Score Score;

        public PlayerBullet Bullet;

        private Animation animationMove;
        private Animation animationDeath;
        private Animation animationIdle;
        private Animation animationShoot;
        private Animation animationJump;

        public Dictionary<string, Animation> animationDictionary;

        public Texture2D ShootTexture;
        public Texture2D IdleTexture;
        public Texture2D DeathTexture;
        public Texture2D StandStillTexture;
        public Texture2D JumpTexture;
        public Texture2D BowDownTexture;

        private bool canMove = true;

        private bool isShootingAnimating = false;

        private bool isShootingCooldown = false;
        private const float ShootingCooldownDuration = 0.5f;
        private float shootingCooldownTimer = 0f;

        private bool isIdling = false;
        private const float IdleTimeoutDuration = 3.0f;
        private float idleTimer = 0f;
        private bool standStillNoIdle = false;

        public AnimationHandler AnimationHandler_MC;

        //TODO: movement enum van dit maken
        bool isMovingLeft;
        bool isMovingRight;
        bool isMovingUp;
        bool isMovingDown;

        // Movement
        //public ManualMovement manualMovementController;
        // 
        //public Input MainCharacterInput;


        // Consistent Hitbox
        public override Rectangle RectangleHitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 34, 44);
            }
        }

        bool hasJumped;

        private const float GravityAcceleration = 125;

        private bool keyPressed;




        public Archeologist(Texture2D moveTexture, Texture2D shootTexture, Texture2D idleTexture, Texture2D deathTexture, Texture2D standStillTexture, Texture2D jumpTexture, Texture2D bowDownTexture)
            : base(moveTexture)
        {
            AnimationHandler_MC = new AnimationHandler();

            StandStillTexture = standStillTexture;

            JumpTexture = jumpTexture;

            BowDownTexture = bowDownTexture;

            hasJumped = true;

            OriginBullet = new Vector2(30, _texture.Height / 2 - 2);



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

            //Otherwise code keeps falling
            PieceOfCodeToFall = 0;

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
                if (sprite is Archeologist)
                {
                    continue;
                }
                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && (sprite is FallingCode || sprite is EnemyBullet) && sprite is NotSentient notSentient)
                {
                    isDeathAnimating = true;
                    notSentient.IsDestroyed = true;
                }

                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is Regular_Point && sprite is NotSentient notSentient2)
                {
                    Score.MainScore++;
                    PieceOfCodeToFall = 5;
                    notSentient2.IsDestroyed = true;
                }
            }

            foreach (var block in blocks)
            {
                if (block is Block && block is not ThreePointsType && block is not SevenPointsType)
                {
                    if (Velocity.X > 0 && IsTouchingLeftBlock(block) && !hasJumped ||
                        Velocity.X < 0 && IsTouchingRightBlock(block) && !hasJumped)
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
                    if ((IsTouchingLeftBlock(block) || IsTouchingTopBlock(block) || IsTouchingTopBlock(block)) && Game1.PlayerScore.MainScore >= 3)
                    {
                        PlayingState.isNextLevelTrigger = true;
                    }
                }
                else if (block is SevenPointsType)
                {
                    if ((IsTouchingLeftBlock(block) || IsTouchingTopBlock(block) || IsTouchingTopBlock(block)) && Game1.PlayerScore.MainScore >= 7)
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
            bullet.BulletSpeed = Speed;
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

                if (Keyboard.GetState().IsKeyDown((Keys)Input.Up))
                {
                    isMovingUp = true;
                }

                if (Keyboard.GetState().IsKeyDown((Keys)Input.Up) && !hasJumped)
                {
                    hasJumped = true;
                    isMovingUp = true;

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
                    /*if (!hasJumped)
                    {
                        float i = 3;
                        Velocity.Y -= 0.45f * i;
                    }*/

                    //Position.Y -= 10f;
                    //Velocity.Y = -62f;
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
                    isMovingDown = true;
                }
                if (Keyboard.GetState().IsKeyDown((Keys)Input.Left) && !hasJumped)
                {
                    Velocity.X -= Speed;
                    facingDirection = -Vector2.UnitX;
                    facingDirectionIndicator = false;
                    isMovingLeft = true;
                }
                if (Keyboard.GetState().IsKeyDown((Keys)Input.Right) && !hasJumped)
                {
                    Velocity.X += Speed;
                    facingDirection = Vector2.UnitX;
                    facingDirectionIndicator = true;
                    isMovingRight = true;

                }
                if (!Keyboard.GetState().IsKeyDown((Keys)Input.Left))
                {
                    isMovingLeft = false;
                }
                if (!Keyboard.GetState().IsKeyDown((Keys)Input.Right))
                {
                    isMovingRight = false;
                }
                if (!Keyboard.GetState().IsKeyDown((Keys)Input.Down))
                {
                    isMovingDown = false;
                }
                if (!Keyboard.GetState().IsKeyDown((Keys)Input.Up))
                {
                    isMovingUp = false;
                }
                if (isDeathAnimating)
                {
                    Velocity = Vector2.Zero;
                }

                Position = Vector2.Clamp(Position, new Vector2(0, 0 + RectangleHitbox.Height / 4), new Vector2(Game1.ScreenWidth - RectangleHitbox.Width, Game1.ScreenHeight - RectangleHitbox.Height));

                animationMove.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDeathAnimating)
            {
                if (facingDirectionIndicator == false)
                {
                    AnimationHandler_MC.DrawAnimation(spriteBatch, animationDictionary["DeathAnimation"], Position, false);
                }
                else if (facingDirectionIndicator == true)
                {
                    AnimationHandler_MC.DrawAnimation(spriteBatch, animationDictionary["DeathAnimation"], Position, true);
                }
                if (animationDictionary["DeathAnimation"].IsAnimationComplete)
                {
                    HasDied = true;
                    IsKilled = true;
                }
            }
            else if (isShootingAnimating)
            {
                if (facingDirectionIndicator == true)
                {
                    AnimationHandler_MC.DrawAnimation(spriteBatch, animationDictionary["AttackAnimation"], Position, true);
                }
                else if (facingDirectionIndicator == false)
                {
                    AnimationHandler_MC.DrawAnimation(spriteBatch, animationDictionary["AttackAnimation"], Position, false);
                }
                if (animationDictionary["AttackAnimation"].IsAnimationComplete)
                {
                    isShootingAnimating = false;
                }
            }
            else if (hasJumped && facingDirectionIndicator == true && !isMovingDown)
            {
                AnimationHandler_MC.DrawAnimation(spriteBatch, animationDictionary["JumpAnimation"], Position, true);
            }
            else if (hasJumped && facingDirectionIndicator == false && !isMovingDown)
            {
                AnimationHandler_MC.DrawAnimation(spriteBatch, animationDictionary["JumpAnimation"], Position, false);
            }
            else if (isMovingLeft)
            {
                AnimationHandler_MC.DrawAnimation(spriteBatch, animationDictionary["MoveAnimation"], Position, false);
            }
            else if (isMovingRight)
            {
                AnimationHandler_MC.DrawAnimation(spriteBatch, animationDictionary["MoveAnimation"], Position, true);
            }
            else if (isIdling)
            {
                if (facingDirectionIndicator == true)
                {
                    AnimationHandler_MC.DrawAnimation(spriteBatch, animationDictionary["IdleAnimation"], Position, true);
                }
                else if (facingDirectionIndicator == false)
                {
                    AnimationHandler_MC.DrawAnimation(spriteBatch, animationDictionary["IdleAnimation"], Position, false);
                }
            }

            else if (facingDirectionIndicator == true && standStillNoIdle == true && !isShootingAnimating || facingDirectionIndicator == true && !isShootingAnimating && !isMovingDown || facingDirectionIndicator == true && !isShootingAnimating && isMovingUp)
            {
                AnimationHandler_MC.DrawOneFrameAnimation(spriteBatch, StandStillTexture, Position, true);
            }
            else if (facingDirectionIndicator == false && standStillNoIdle == true && !isShootingAnimating || facingDirectionIndicator == false && !isShootingAnimating && isMovingUp || facingDirectionIndicator == false && !isShootingAnimating && !isMovingDown)
            {
                AnimationHandler_MC.DrawOneFrameAnimation(spriteBatch, StandStillTexture, Position, false);
            }
            else if (facingDirectionIndicator == true && isMovingDown && !hasJumped)
            {
                AnimationHandler_MC.DrawOneFrameAnimation(spriteBatch, BowDownTexture, Position + new Vector2(0, 2), true);
            }
            else if (facingDirectionIndicator == false && isMovingDown && !hasJumped)
            {
                AnimationHandler_MC.DrawOneFrameAnimation(spriteBatch, BowDownTexture, Position + new Vector2(0, 2), false);
            }
            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);

        }
    }
}