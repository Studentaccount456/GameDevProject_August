using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using GameDevProject_August.UI;
using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Sprites.Sentient.Characters.Enemy;
using GameDevProject_August.Sprites.NotSentient.Projectiles;
using GameDevProject_August.Sprites.NotSentient.Collectibles;
using GameDevProject_August.Levels;
using SharpDX.Direct2D1.Effects;
using GameDevProject_August.AnimationClasses.AnimationMethods;
using GameDevProject_August.Models;
using SharpDX.Direct3D9;

namespace GameDevProject_August.Sprites.Sentient.Characters.Main
{
    public class MainCharacter : Sprite
    {
        public Score Score;

        public PlayerBullet Bullet;

        public bool HasDied = false;

        private Animation animationMove;
        private Animation animationDeath;
        private Animation animationIdle;
        private Animation animationShoot;

        public Dictionary<string, Animation> animationDictionary;

        public Texture2D ShootTexture;
        public Texture2D IdleTexture;
        public Texture2D DeathTexture;
        public Texture2D StandStillTexture;

        private bool canMove = true;

        private bool isDeathAnimating = false;
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

        public CharacterDrawer CharacterDrawer_MC;

        // Movement
        //public ManualMovement manualMovementController;
        // 
        //public Input MainCharacterInput;


        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 34, 44);
            }
        }

        public MainCharacter(Texture2D moveTexture, Texture2D shootTexture, Texture2D idleTexture, Texture2D deathTexture, Texture2D standStillTexture)
            : base(moveTexture)
        {
            AnimationHandler_MC = new AnimationHandler();

            CharacterDrawer_MC = new CharacterDrawer(AnimationHandler_MC);

            StandStillTexture = standStillTexture;

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
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(0, 0, 32, 44)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(128, 0, 42, 44)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(260, 0, 50, 44)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(388, 0, 50, 44)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(516, 0, 50, 44)));
            #endregion

            animationDictionary = new Dictionary<string, Animation>
{
                                { "MoveAnimation", animationMove },
                                { "AttackAnimation", animationShoot },
                                { "DeathAnimation", animationDeath },
                                { "IdleAnimation", animationIdle }
                                };
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            bool keyPressed = _currentKey.IsKeyDown(Keys.Left) || _currentKey.IsKeyDown(Keys.Right) ||
                  _currentKey.IsKeyDown(Keys.Up) || _currentKey.IsKeyDown(Keys.Down) ||
                  _currentKey.IsKeyDown(Keys.Space) && isShootingAnimating;

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
                } else
                {
                    standStillNoIdle = true;
                }
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
                if (animationShoot.IsAnimationComplete)
                {
                    isShootingAnimating = false;
                }
            }
            else
            {
                if (canMove)
                {
                    //manualMovementController.Move(Position, Rectangle, Velocity, Speed, facingDirection, facingDirectionIndicator);
                    Move();
                    animationMove.Update(gameTime);

                }
            }

            if (_currentKey.IsKeyDown(Keys.Space) && _previousKey.IsKeyUp(Keys.Space) && !isShootingCooldown && !isShootingAnimating)
            {
                AddBullet(sprites);
                isShootingAnimating = true; 

                isShootingCooldown = true;
                shootingCooldownTimer = 0f;
            }

            foreach (var sprite in sprites)
            {
                if (sprite is MainCharacter)
                {
                    continue;
                }

                if (sprite.Rectangle.Intersects(Rectangle) && sprite is FallingCode)
                {
                    HasDied = true;
                    isDeathAnimating = true;
                    sprite.IsRemoved = true;
                }

                if (sprite.Rectangle.Intersects(Rectangle) && sprite is Regular_Point)
                {
                    Score.MainScore++;
                    sprite.IsRemoved = true;
                }

                if (sprite is Component)
                {
                    if (Velocity.X > 0 && IsTouchingLeft(sprite) ||
                        Velocity.X < 0 && IsTouchingRight(sprite))
                    {
                        Velocity.X = 0;
                    }

                    if (Velocity.Y > 0 && IsTouchingTop(sprite) ||
                        Velocity.Y < 0 && IsTouchingBottom(sprite))
                    {
                        Velocity.Y = 0;
                    }

                }
            }

            foreach (var block in blocks)
            {
                if (block is Block)
                {
                    if (Velocity.X > 0 && IsTouchingLeftBlock(block) ||
                        Velocity.X < 0 && IsTouchingRightBlock(block))
                    {
                        Velocity.X = 0;
                    }

                    if (Velocity.Y > 0 && IsTouchingTopBlock(block) ||
                        Velocity.Y < 0 && IsTouchingBottomBlock(block))
                    {
                        Velocity.Y = 0;
                    }

                }
            }

            Position += Velocity;

            Velocity = Vector2.Zero;
            animationShoot.Update(gameTime);
            if (!isShootingAnimating)
            {
                    animationIdle.Update(gameTime);
            }
            if (isDeathAnimating == true) 
            {
                animationDeath.Update(gameTime);
            }
        }


        private void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as PlayerBullet;
            bullet.facingDirection = facingDirection;
            bullet.Position = Position + OriginBullet;
            bullet.Speed = Speed;
            bullet.Lifespan = 1f;
            bullet.Parent = this;

            sprites.Add(bullet);
        }

        private void Move()
        {
            if (Input == null)
                return;

            if (Keyboard.GetState().IsKeyDown((Keys)Input.Up))
            {
                Velocity.Y -= Speed;
                isMovingUp = true;
            }
            if (Keyboard.GetState().IsKeyDown((Keys)Input.Down))
            {
                Velocity.Y += Speed;
                isMovingDown = true;
            }
            if (Keyboard.GetState().IsKeyDown((Keys)Input.Left))
            {
                Velocity.X -= Speed;
                facingDirection = -Vector2.UnitX;
                facingDirectionIndicator = false;
                isMovingLeft = true;
            }
            if (Keyboard.GetState().IsKeyDown((Keys)Input.Right))
            {
                Velocity.X += Speed;
                facingDirection = Vector2.UnitX;
                facingDirectionIndicator = true; 
                isMovingRight = true;

            }
            if (!Keyboard.GetState().IsKeyDown((Keys)Input.Left)) {
                isMovingLeft = false;
            }
            if (!Keyboard.GetState().IsKeyDown((Keys)Input.Right)) {
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

            Position = Vector2.Clamp(Position, new Vector2(0, 0 + Rectangle.Height / 4), new Vector2(Game1.ScreenWidth - Rectangle.Width, Game1.ScreenHeight - Rectangle.Height));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CharacterDrawer_MC.DrawMainCharacter(spriteBatch, AnimationHandler_MC, animationDictionary, StandStillTexture, Position, isDeathAnimating, isShootingAnimating, facingDirectionIndicator, 
                isIdling, standStillNoIdle, isMovingLeft, isMovingRight, isMovingUp, isMovingDown);

            spriteBatch.DrawRectangle(Rectangle, Color.Blue);
        }
    }
}
