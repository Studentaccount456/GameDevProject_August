using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Models;
using GameDevProject_August.Sprites.NotSentient.Collectibles;
using GameDevProject_August.Sprites.NotSentient.Projectiles;
using GameDevProject_August.Sprites.Sentient.Characters.Main;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using GameDevProject_August.UI;
using System;
using GameDevProject_August.AnimationClasses.AnimationMethods;
using GameDevProject_August.Levels;
using GameDevProject_August.States;

namespace GameDevProject_August.Sprites.Sentient.Characters.Enemy
{
    public class Minotaur : Sprite
    {
        public bool HasDied = false;

        private Animation animationMove;
        private Animation animationDeath;
        private Animation animationIdle;
        private Animation animationShoot;

        public Texture2D ShootTexture;
        public Texture2D IdleTexture;
        public Texture2D DeathTexture;
        public Texture2D StandStillTexture;

        private bool canMove = true;
        private bool isMoving = false;

        private bool isDeathAnimating = false;
        private bool isShootingAnimating = false;

        private bool isShootingCooldown = false;
        private const float ShootingCooldownDuration = 0.5f;
        private float shootingCooldownTimer = 0f;

        private bool isIdling = false;
        private const float IdleTimeoutDuration = 5.0f;
        private float idleTimer = 0f;
        private bool standStillNoIdle = false;

        private int deathAnimationFrameIndex = 0;
        private int shootAnimationFrameIndex = 0;


        private Vector2 OffsetAnimation;

        private bool reachedFourthDeathFrame = false;

        // Tongue Rectangle
        public Rectangle Rectangle2;

        public Rectangle DeathRectangle;

        public AnimationHandler AnimationHandler_Minotaur;

        //TODO: movement enum van dit maken
        bool isMovingLeft;
        bool isMovingRight;
        bool isMovingUp;
        bool isMovingDown;

        /*
        public override Rectangle RectangleHitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 63, 42);
            }
        }
        */

        private const float GravityAcceleration = 9.81f;



        public Minotaur(Texture2D moveTexture, Texture2D shootTexture, Texture2D idleTexture, Texture2D deathTexture, Texture2D standStillTexture)
            : base(moveTexture)
        {
            _texture = moveTexture;
            ShootTexture = shootTexture;
            IdleTexture = idleTexture;
            DeathTexture = deathTexture;
            StandStillTexture = standStillTexture;

            AnimationHandler_Minotaur = new AnimationHandler();

            Rectangle2 = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);


            #region MoveAnimation
            animationMove = new Animation(AnimationType.Move, moveTexture);
            animationMove.AddFrame(new AnimationFrame(new Rectangle(0, 0, 54, 51)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(96, 0, 54, 51)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(192, 0, 54, 51)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(288, 0, 54, 51)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(384, 0, 54, 51)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(480, 0, 54, 51)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(576, 0, 54, 51)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(672, 0, 54, 51)));
            #endregion

            // Height for standstill without attack is 42
            OffsetAnimation = new Vector2(0, -30);
            #region animationShoot
            animationShoot = new Animation(AnimationType.Attack, shootTexture);
            animationShoot.fps = 1;
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(0, 0, 63, 72)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(96, 0, 69, 72)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(192, 0, 75, 72)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(288, 0, 75, 72)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(384, 0, 72, 72)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(480, 0, 72, 72)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(576, 0, 75, 72)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(672, 0, 72, 72)));
            #endregion

            #region Idle
            animationIdle = new Animation(AnimationType.Idle, idleTexture);
            animationIdle.fps = 8;
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(0, 0, 63, 42)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(96, 0, 63, 42)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(192, 0, 63, 42)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(288, 0, 66, 42)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(384, 0, 66, 42)));
            #endregion

            #region Death
            animationDeath = new Animation(AnimationType.Death, deathTexture);
            animationDeath.fps = 8;
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
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();
            if (!isDeathAnimating)
            {
                //Velocity.Y += GravityAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            PositionXRectangleHitbox = (int)Position.X;
            PositionYRectangleHitbox = (int)Position.Y;


            bool keyPressed = Keyboard.GetState().IsKeyDown((Keys)Input.Left) || Keyboard.GetState().IsKeyDown((Keys)Input.Right) ||
                  Keyboard.GetState().IsKeyDown((Keys)Input.Up) || Keyboard.GetState().IsKeyDown((Keys)Input.Down) ||
                  Keyboard.GetState().IsKeyDown((Keys)Input.Shoot) && isShootingAnimating;

            bool isMoving = Keyboard.GetState().IsKeyDown((Keys)Input.Left) || Keyboard.GetState().IsKeyDown((Keys)Input.Right) ||
                  Keyboard.GetState().IsKeyDown((Keys)Input.Up) || Keyboard.GetState().IsKeyDown((Keys)Input.Down);

            if (isMoving)
            {
                WidthRectangleHitbox = 54;
                HeightRectangleHitbox = 42;
                PositionYRectangleHitbox = (int)Position.Y;
            }
            else
            {
                WidthRectangleHitbox = 63;
                HeightRectangleHitbox = 42;
            }

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
                }
                else
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
                PositionXRectangleHitbox = (int)Position.X;
                PositionYRectangleHitbox = (int)Position.Y;
                WidthRectangleHitbox = 63;
                HeightRectangleHitbox = 42;
                Rectangle2.Width = 0;
                Rectangle2.Height = 0;
                shootAnimationFrameIndex = animationShoot.CurrentFrameIndex;
                if (shootAnimationFrameIndex == 1)
                {
                    WidthRectangleHitbox = 69;
                    HeightRectangleHitbox = 42;
                }
                else if (shootAnimationFrameIndex == 2 || shootAnimationFrameIndex == 3 || shootAnimationFrameIndex == 6)
                {
                    WidthRectangleHitbox = 75;
                    HeightRectangleHitbox = 42;
                }
                else if (shootAnimationFrameIndex == 4)
                {
                    WidthRectangleHitbox = 54;
                    HeightRectangleHitbox = 42;
                    //Tongue
                    if (facingDirectionIndicator)
                    {
                        Rectangle2.X = (int)Position.X + 45;
                    }
                    else
                    {
                        Rectangle2.X = (int)Position.X;
                    }
                    Rectangle2.Y = (int)Position.Y - 30;
                    Rectangle2.Width = 27;
                    Rectangle2.Height = 30;
                }
                else if (shootAnimationFrameIndex == 5)
                {
                    WidthRectangleHitbox = 69;
                    HeightRectangleHitbox = 42;
                    //Tongue
                    if (facingDirectionIndicator)
                    {
                        Rectangle2.X = (int)Position.X + 63;
                    }
                    else
                    {
                        Rectangle2.X = (int)Position.X;
                    }
                    Rectangle2.Y = (int)Position.Y - 15;
                    Rectangle2.Width = 9;
                    Rectangle2.Height = 15;
                }
                else if (shootAnimationFrameIndex == 7)
                {
                    WidthRectangleHitbox = 72;
                    HeightRectangleHitbox = 42;
                }
                //RectangleHitBox.Width = 54;
                //RectangleHitBox.Height = 42;
                animationShoot.Update(gameTime);
                if (animationShoot.IsAnimationComplete)
                {
                    isShootingAnimating = false;
                }
            }
            else
            {
                if (canMove && !isDeathAnimating)
                {
                    Move();
                    animationMove.Update(gameTime);
                }
            }

            if (_currentKey.IsKeyDown(Keys.M) && _previousKey.IsKeyUp(Keys.M) && !isShootingCooldown && !isShootingAnimating)
            {
                isShootingAnimating = true;

                isShootingCooldown = true;
                shootingCooldownTimer = 0f;
            }

            foreach (var sprite in sprites)
            {
                if (sprite is Minotaur)
                {
                    continue;
                }

                if ((sprite.RectangleHitbox.Intersects(RectangleHitbox) || sprite.RectangleHitbox.Intersects(Rectangle2)) && sprite is MainCharacter)
                {
                    HasDied = true;
                    sprite.isDeathAnimating = true;
                }

                if ((sprite.RectangleHitbox.Intersects(RectangleHitbox) || sprite.RectangleHitbox.Intersects(Rectangle2)) && sprite is PlayerBullet)
                {
                    Game1.PlayerScore.MainScore++;
                    HasDied = true;
                    isDeathAnimating = true;
                    sprite.IsRemoved = true;
                }


                if (isDeathAnimating)
                {
                    DeathRectangle = new Rectangle((int)Position.X, (int)Position.Y, 64, 64);
                    WidthRectangleHitbox = 0;
                    HeightRectangleHitbox = 0;

                    animationDeath.Update(gameTime);

                    deathAnimationFrameIndex = animationDeath.CurrentFrameIndex;

                    if (deathAnimationFrameIndex == 3) // 4th frame
                    {
                        reachedFourthDeathFrame = true;
                    }

                    if (reachedFourthDeathFrame && animationDeath.IsAnimationComplete)
                    {
                        PieceOfCodeToFall = 1;
                        IsRemoved = true;
                    }

                    /*if (sprite.RectangleHitbox.Intersects(DeathRectangle) && sprite is MainCharacter)
                    {
                        sprite.HasDied = true;
                    }
                    */
                    if (deathAnimationFrameIndex > 6)
                    {
                        DeathRectangle.Width = 0;
                        DeathRectangle.Height = 0;
                    }

                }


                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is Regular_Point)
                {
                    sprite.IsRemoved = true;
                }

                if (sprite.RectangleHitbox.Intersects(DeathRectangle) && sprite is MainCharacter)
                {
                    sprite.isDeathAnimating = true;
                }

                //TODO CHANGE IF SPRITE IS ENEMY
                /*
                if (sprite is not Minotaur)
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
                */

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
        }

        private void Move()
        {
            if (Input == null)
                return;

            if (Keyboard.GetState().IsKeyDown((Keys)Input.Up))
            {
                Velocity.Y -= Speed;
            }
            if (Keyboard.GetState().IsKeyDown((Keys)Input.Down))
            {
                Velocity.Y += Speed;
            }
            if (Keyboard.GetState().IsKeyDown((Keys)Input.Left))
            {
                Velocity.X -= Speed;
                facingDirection = -Vector2.UnitX;
                facingDirectionIndicator = false;
            }
            if (Keyboard.GetState().IsKeyDown((Keys)Input.Right))
            {
                Velocity.X += Speed;
                facingDirection = Vector2.UnitX;
                facingDirectionIndicator = true;
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


            Position = Vector2.Clamp(Position, new Vector2(0 - RectangleHitbox.Width, 0 + RectangleHitbox.Height / 2), new Vector2(Game1.ScreenWidth - RectangleHitbox.Width, Game1.ScreenHeight - RectangleHitbox.Height / 2));
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDeathAnimating)
            {
                if (animationDeath.IsAnimationComplete)
                {
                    IsRemoved = true;
                }
                AnimationHandler_Minotaur.DrawAnimation(spriteBatch, animationDeath, Position, true);
            }
            else if (isShootingAnimating)
            {
                if (facingDirectionIndicator == true)
                {
                    spriteBatch.Draw(ShootTexture, Position + OffsetAnimation, animationShoot.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
                }
                else if (facingDirectionIndicator == false)
                {
                    spriteBatch.Draw(ShootTexture, Position + OffsetAnimation, animationShoot.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
                }
                if (animationShoot.IsAnimationComplete)
                {
                    isShootingAnimating = false;
                }
            }
            else if (Keyboard.GetState().IsKeyDown((Keys)Input.Right))
            {
                spriteBatch.Draw(_texture, Position + new Vector2(0, -8), animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (Keyboard.GetState().IsKeyDown((Keys)Input.Left))
            {
                spriteBatch.Draw(_texture, Position + new Vector2(0, -8), animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }
            else if (isIdling)
            {
                spriteBatch.Draw(IdleTexture, Position, animationIdle.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (facingDirectionIndicator == true && standStillNoIdle == true && !isShootingAnimating)
            {
                spriteBatch.Draw(StandStillTexture, Position, null, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (facingDirectionIndicator == false && standStillNoIdle == true && !isShootingAnimating)
            {
                spriteBatch.Draw(StandStillTexture, Position, null, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }
            else if (facingDirectionIndicator == true && standStillNoIdle == true && !isShootingAnimating || (facingDirectionIndicator == true && !isShootingAnimating && isMovingDown) || (facingDirectionIndicator == true && !isShootingAnimating && isMovingUp))
            {
                AnimationHandler_Minotaur.DrawOneFrameAnimation(spriteBatch, StandStillTexture, Position, true);
            }
            else if (facingDirectionIndicator == false && standStillNoIdle == true && !isShootingAnimating || (facingDirectionIndicator == false && !isShootingAnimating && isMovingUp) || (facingDirectionIndicator == false && !isShootingAnimating && isMovingDown))
            {
                AnimationHandler_Minotaur.DrawOneFrameAnimation(spriteBatch, StandStillTexture, Position, false);
            }

            spriteBatch.DrawRectangle(Rectangle2, Color.Blue);
            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
            spriteBatch.DrawRectangle(DeathRectangle, Color.Red);

        }

    }
}
