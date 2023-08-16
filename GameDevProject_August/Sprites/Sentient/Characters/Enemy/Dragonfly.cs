﻿using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Levels;
using GameDevProject_August.Sprites.NotSentient.Projectiles;
using GameDevProject_August.Sprites.Sentient.Characters.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.Sentient.Characters.Enemy
{
    public class Dragonfly : Sprite
    {
        public bool HasDied = false;

        private Animation animationMove;
        private Animation animationDeath;

        public Texture2D DeathTexture;

        private bool isDeathAnimating = false;

        private int deathAnimationFrameIndex = 0;

        private bool reachedFourthDeathFrame = false;

        private bool isMovingUp = true;



        public override Rectangle RectangleHitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 51, 39);
            }
        }


        public Rectangle DeathRectangle;

        public Dragonfly(Texture2D moveTexture, Texture2D deathTexture)
            : base(moveTexture)
        {
            _texture = moveTexture;
            DeathTexture = deathTexture;
            facingDirectionIndicator = false;

            // Standard walks right
            #region MoveAnimation
            animationMove = new Animation(AnimationType.Move, moveTexture);
            animationMove.fps = 8;
            animationMove.AddFrame(new AnimationFrame(new Rectangle(0, 0, 51, 42)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(96, 0, 51, 42)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(192, 0, 51, 42)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(288, 0, 51, 42)));
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
            if (!isDeathAnimating)
            {
                Move();
                animationMove.Update(gameTime);
            }

            foreach (var sprite in sprites)
            {
                if (sprite is Dragonfly)
                {
                    continue;
                }

                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is MainCharacter)
                {
                    HasDied = true;
                    sprite.isDeathAnimating = true;
                }

                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is PlayerBullet)
                {
                    Game1.PlayerScore.MainScore++;
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
                        PieceOfCodeToFall = 2;
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

                Position += Velocity;

                Velocity = Vector2.Zero;
                if (isDeathAnimating == true)
                {
                    animationDeath.Update(gameTime);
                }
            }

            foreach (var block in blocks)
            {
                if (block.BlockRectangle.Intersects(RectangleHitbox) && block.EnemyBehavior == true)
                {
                    isMovingUp = !isMovingUp;
                }
            }
        }

        private void Move()
        {
            if (isMovingUp)
            {
                Velocity.Y -= Speed;
            }
            if (!isMovingUp)
            {
                Velocity.Y += Speed;
            }

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
            else if (isMovingUp || !isMovingUp)
            {
                spriteBatch.Draw(_texture, Position, animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }

            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
            spriteBatch.DrawRectangle(DeathRectangle, Color.Red);
        }

    }
}
