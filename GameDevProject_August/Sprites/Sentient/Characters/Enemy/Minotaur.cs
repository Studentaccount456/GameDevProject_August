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

        private bool isDeathAnimating = false;
        private bool isShootingAnimating = false;

        private bool isShootingCooldown = false;
        private const float ShootingCooldownDuration = 0.5f;
        private float shootingCooldownTimer = 0f;

        private bool isIdling = false;
        private const float IdleTimeoutDuration = 3.0f;
        private float idleTimer = 0f;
        private bool standStillNoIdle = false;

        private int deathAnimationFrameIndex = 0;
        private int shootAnimationFrameIndex = 0;


        private Vector2 OffsetAnimation;

        private bool reachedFourthDeathFrame = false;




        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 5, 5);
            }
        }

        // Tongue Rectangle
        public Rectangle Rectangle2;

        public Rectangle DeathRectangle;

        public Rectangle RectangleHitBox;

        public AnimationHandler AnimationHandler_Minotaur;


        public Minotaur(Texture2D moveTexture, Texture2D shootTexture, Texture2D idleTexture, Texture2D deathTexture, Texture2D standStillTexture)
            : base(moveTexture)
        {
            _texture = moveTexture;
            ShootTexture = shootTexture;
            IdleTexture = idleTexture;
            DeathTexture = deathTexture;
            StandStillTexture = standStillTexture;

            AnimationHandler_Minotaur = new AnimationHandler();

            //Move
            RectangleHitBox = new Rectangle((int)Position.X, (int)Position.Y, 54, 51);
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

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();
            RectangleHitBox.X = (int)Position.X;
            RectangleHitBox.Y = (int)Position.Y;


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
                    RectangleHitBox.Width = 64;
                    RectangleHitBox.Height = 42;
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
                RectangleHitBox.X = (int)Position.X;
                RectangleHitBox.Y = (int)Position.Y;
                RectangleHitBox.Width = 63;
                RectangleHitBox.Height = 42;
                Rectangle2.Width = 0;
                Rectangle2.Height = 0;
                shootAnimationFrameIndex = animationShoot.CurrentFrameIndex;
                if (shootAnimationFrameIndex == 1)
                {
                    RectangleHitBox.Width = 69;
                    RectangleHitBox.Height = 42;
                }
                else if (shootAnimationFrameIndex == 2 || shootAnimationFrameIndex == 3 || shootAnimationFrameIndex == 6)
                {
                    RectangleHitBox.Width = 75;
                    RectangleHitBox.Height = 42;
                }
                else if (shootAnimationFrameIndex == 4)
                {
                    RectangleHitBox.Width = 54;
                    RectangleHitBox.Height = 42;
                    //Tongue
                    if(facingDirectionIndicator)
                    {
                        Rectangle2.X = (int)Position.X + 45;
                    } else
                    {
                        Rectangle2.X = (int)Position.X;
                    }
                    Rectangle2.Y = (int)Position.Y - 30;
                    Rectangle2.Width = 27;
                    Rectangle2.Height = 30;
                }
                else if (shootAnimationFrameIndex == 5)
                {
                    RectangleHitBox.Width = 69;
                    RectangleHitBox.Height = 42;
                    //Tongue
                    if(facingDirectionIndicator)
                    {
                        Rectangle2.X = (int)Position.X + 63;
                    } else
                    {
                        Rectangle2.X = (int)Position.X;
                    }
                    Rectangle2.Y = (int)Position.Y - 15;
                    Rectangle2.Width = 9;
                    Rectangle2.Height = 15;
                }
                else if (shootAnimationFrameIndex == 7)
                {
                    RectangleHitBox.Width = 72;
                    RectangleHitBox.Height = 42;
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
                    RectangleHitBox.Width =  54;
                    RectangleHitBox.Height = 47;

                }
            }

            if (_currentKey.IsKeyDown(Keys.Space) && _previousKey.IsKeyUp(Keys.Space) && !isShootingCooldown && !isShootingAnimating)
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

                if ((sprite.Rectangle.Intersects(RectangleHitBox) || sprite.Rectangle.Intersects(Rectangle2)) && sprite is MainCharacter)
                {
                    HasDied = true;
                    sprite.IsRemoved = true;
                }

                if ((sprite.Rectangle.Intersects(RectangleHitBox) || sprite.Rectangle.Intersects(Rectangle2)) && sprite is PlayerBullet)
                {
                    HasDied = true;
                    isDeathAnimating = true;
                    sprite.IsRemoved = true;
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
                        IsRemoved = true;
                    }

                    if (sprite.Rectangle.Intersects(DeathRectangle) && sprite is MainCharacter)
                    {
                        sprite.IsRemoved = true;
                    }

                    if (deathAnimationFrameIndex > 6)
                    {
                        DeathRectangle.Width = 0;
                        DeathRectangle.Height = 0;
                    }

                }

                if (sprite.Rectangle.Intersects(Rectangle) && sprite is Regular_Point)
                {
                    sprite.IsRemoved = true;
                }

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


            /*
            if (isDeathAnimating)
            {
                animationDeath.Update(gameTime);

                deathAnimationFrameIndex = animationDeath.CurrentFrameIndex;

                if (deathAnimationFrameIndex == 3) // 4th frame
                {
                    IsRemoved = true;
                }
            }
            */

            // Update Rectangle2 when facingDirection Changes
            int rect2X = (int)Position.X + 42;

            if (!facingDirectionIndicator)
            {
                // Left-Facing direction 
                rect2X -= 27;
            }

            // Update Rectangle2
            //Rectangle2 = new Rectangle(rect3X, (int)Position.Y, 42, 48);

            Position = Vector2.Clamp(Position, new Vector2(0 - Rectangle.Width, 0 + Rectangle.Height / 2), new Vector2(Game1.ScreenWidth - Rectangle.Width, Game1.ScreenHeight - Rectangle.Height / 2));
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDeathAnimating)
            {
                if (reachedFourthDeathFrame)
                {
                    //AnimationHandler_Minotaur.PlayMoveAnimation(spriteBatch, animationDeath, Position, true);
                    //spriteBatch.Draw(DeathTexture, Position, animationDeath.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
                }
                else
                {
                    //AnimationHandler_Minotaur.PlayMoveAnimation(spriteBatch, animationDeath, Position, true);
                    //spriteBatch.Draw(DeathTexture, Position, animationDeath.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
                    if (animationDeath.IsAnimationComplete)
                    {
                        IsRemoved = true;
                    }
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
                spriteBatch.Draw(_texture, Position, animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (Keyboard.GetState().IsKeyDown((Keys)Input.Left))
            {
                spriteBatch.Draw(_texture, Position, animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
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

            spriteBatch.DrawRectangle(Rectangle2, Color.Blue);
            spriteBatch.DrawRectangle(RectangleHitBox, Color.Blue);

        }

    }
}
