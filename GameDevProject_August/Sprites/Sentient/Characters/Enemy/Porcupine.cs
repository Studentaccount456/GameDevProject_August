using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Models;
using GameDevProject_August.Sprites.NotSentient.Collectibles;
using GameDevProject_August.Sprites.NotSentient.Projectiles;
using GameDevProject_August.Sprites.Sentient.Characters.Main;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using GameDevProject_August.UI;
using SharpDX.Direct3D9;
using GameDevProject_August.Levels;

namespace GameDevProject_August.Sprites.Sentient.Characters.Enemy
{
    public class Porcupine : Sprite
    {
        public bool HasDied = false;

        private Animation animationMove;
        private Animation animationDeath;

        public Texture2D DeathTexture;
        public Texture2D StandStillTexture;

        private bool canMove = true;

        private bool isDeathAnimating = false;

        private int deathAnimationFrameIndex = 0;

        private bool reachedFourthDeathFrame = false;


        /*
        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 57, 48);
            }
        }
        */

        public Rectangle Rectangle2;
        public Rectangle Rectangle3;

        public Rectangle DeathRectangle;

        public Porcupine(Texture2D moveTexture, Texture2D deathTexture, Texture2D standStillTexture)
            : base(moveTexture)
        {
            _texture = moveTexture;
            DeathTexture = deathTexture;
            StandStillTexture = standStillTexture;


            // Standard walks right
            #region MoveAnimation
            animationMove = new Animation(AnimationType.Move, moveTexture);
            animationMove.fps = 8;
            animationMove.AddFrame(new AnimationFrame(new Rectangle(0, 0, 57, 48)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(96, 0, 57, 48)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(192, 0, 57, 48)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(288, 0, 57, 48)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(384, 0, 57, 48)));
            #endregion

            //Height is 44 for each frame
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
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            bool keyPressed = _currentKey.IsKeyDown(Keys.Left) || _currentKey.IsKeyDown(Keys.Right) ||
                  _currentKey.IsKeyDown(Keys.Up) || _currentKey.IsKeyDown(Keys.Down);

            if (canMove && !isDeathAnimating)
                {
                    Move();
                    animationMove.Update(gameTime);
                }

            foreach (var sprite in sprites)
            {
                if (sprite is Porcupine)
                {
                    continue;
                }

                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is MainCharacter)
                {
                    HasDied = true;
                    sprite.IsRemoved = true;
                }

                if (sprite.RectangleHitbox.Intersects(Rectangle2) && sprite is PlayerBullet)
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
                        PieceOfCodeToFall = 3;
                        IsRemoved = true;
                    }

                    if (sprite.RectangleHitbox.Intersects(DeathRectangle) && sprite is MainCharacter)
                    {
                        sprite.IsRemoved = true;
                    }

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

                if ((sprite.RectangleHitbox.Intersects(Rectangle3) && sprite is PlayerBullet))
                {
                    sprite.IsRemoved = true;
                }


                if (sprite is not Porcupine && sprite is not Regular_Point)
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
            if (isDeathAnimating == true)
            {
                animationDeath.Update(gameTime);
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

            // Update Rectangle2 when facingDirection Changes
            int rect2X = (int)Position.X + 42;
            int rect3X = (int)Position.X;

            if (!facingDirectionIndicator)
            {
                // Left-Facing direction 
                rect2X -= 42;
                rect3X += 16;
            }

            // Update Rectangle2
            Rectangle2 = new Rectangle(rect2X, (int)Position.Y + 24, 15, 24);
            Rectangle3 = new Rectangle(rect3X, (int)Position.Y, 42, 48);


            Position = Vector2.Clamp(Position, new Vector2(0 - RectangleHitbox.Width, 0 + RectangleHitbox.Height / 2), new Vector2(Game1.ScreenWidth - RectangleHitbox.Width, Game1.ScreenHeight - RectangleHitbox.Height / 2));
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
            else if (Keyboard.GetState().IsKeyDown((Keys)Input.Right))
            {
                spriteBatch.Draw(_texture, Position, animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (Keyboard.GetState().IsKeyDown((Keys)Input.Left))
            {
                spriteBatch.Draw(_texture, Position, animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }
            else if (facingDirectionIndicator == true && !Keyboard.GetState().IsKeyDown((Keys)Input.Right))
            {
                spriteBatch.Draw(StandStillTexture, Position, null, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (facingDirectionIndicator == false && !Keyboard.GetState().IsKeyDown((Keys)Input.Right))
            {
                spriteBatch.Draw(StandStillTexture, Position, null, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }

            spriteBatch.DrawRectangle(Rectangle2, Color.Blue);
            spriteBatch.DrawRectangle(Rectangle3, Color.Yellow);
            spriteBatch.DrawRectangle(DeathRectangle, Color.Red);
        }

    }
}
