using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Levels;
using GameDevProject_August.Sprites.DNotSentient;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles;
using GameDevProject_August.Sprites.DSentient;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy
{
    public class Porcupine : Sentient
    {
        private Animation animationMove;
        private Animation animationDeath;

        public Texture2D DeathTexture;

        private int deathAnimationFrameIndex = 0;

        private bool reachedFourthDeathFrame = false;

        public Rectangle AdditionalHitBox_1;

        public Rectangle DeathRectangle;

        public Porcupine(Texture2D moveTexture, Texture2D deathTexture)
            : base(moveTexture)
        {
            _texture = moveTexture;
            DeathTexture = deathTexture;

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
            Move(gameTime, blocks);
            PorcupineHitBoxFunct();

            CollisionRules(gameTime, sprites);

            UpdatePositionAndResetVelocity();
        }

        private void UpdatePositionAndResetVelocity()
        {
            Position += Velocity;

            Velocity = Vector2.Zero;
        }

        private void CollisionRules(GameTime gameTime, List<Sprite> sprites)
        {
            foreach (var sprite in sprites)
            {
                if (sprite is Porcupine)
                {
                    continue;
                }

                if ((sprite.RectangleHitbox.Intersects(RectangleHitbox) || sprite.RectangleHitbox.Intersects(AdditionalHitBox_1)) && sprite is MainCharacter && sprite is Sentient sentient)
                {
                    sentient.isDeathAnimating = true;
                }
                GlitchDeathInit(gameTime, sprite, 3);

                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is PlayerBullet && sprite is NotSentient notSentient)
                {
                    Game1.PlayerScore.MainScore++;
                    isDeathAnimating = true;
                    notSentient.IsDestroyed = true;
                }

                if (sprite.RectangleHitbox.Intersects(AdditionalHitBox_1) && sprite is PlayerBullet && sprite is NotSentient notSentient2)
                {
                    notSentient2.IsDestroyed = true;
                }
            }
        }

        private void GlitchDeathInit(GameTime gameTime, Sprite sprite, int pieceOfCodeToFall)
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
                    PieceOfCodeToFall = pieceOfCodeToFall;
                    IsKilled = true;
                }

                if (sprite.RectangleHitbox.Intersects(DeathRectangle) && sprite is MainCharacter && sprite is Sentient sentient)
                {
                    sentient.isDeathAnimating = true;
                }

                if (deathAnimationFrameIndex > 6)
                {
                    DeathRectangle.Width = 0;
                    DeathRectangle.Height = 0;
                    WidthRectangleHitbox = 0;
                    HeightRectangleHitbox = 0;
                }

            }
        }

        private void Move(GameTime gameTime, List<Block> blocks)
        {
            foreach (var block in blocks)
            {
                if (block.BlockRectangle.Intersects(RectangleHitbox) && block.EnemyBehavior == true)
                {
                    facingDirectionIndicator = !facingDirectionIndicator;
                }
            }

            if (!isDeathAnimating)
            {
                if (!facingDirectionIndicator)
                {
                    Velocity.X -= Speed;
                    facingDirection = -Vector2.UnitX;
                }
                else if (facingDirectionIndicator)
                {
                    Velocity.X += Speed;
                    facingDirection = Vector2.UnitX;
                }

                Position = Vector2.Clamp(Position, new Vector2(0 - RectangleHitbox.Width, 0 + RectangleHitbox.Height / 2), new Vector2(Game1.ScreenWidth - RectangleHitbox.Width, Game1.ScreenHeight - RectangleHitbox.Height / 2));
                animationMove.Update(gameTime);
            }
        }

        private void PorcupineHitBoxFunct()
        {
            // Update Hitboxes when facingDirection Changes
            int rect2X = (int)Position.X + 42;
            int rect3X = (int)Position.X;

            if (!facingDirectionIndicator)
            {
                // Left-Facing direction 
                rect2X -= 42;
                rect3X += 16;
            }

            // Update Hitboxes
            PositionXRectangleHitbox = rect2X;
            PositionYRectangleHitbox = (int)Position.Y + 24;
            WidthRectangleHitbox = 15;
            HeightRectangleHitbox = 24;
            AdditionalHitBox_1 = new Rectangle(rect3X, (int)Position.Y, 42, 48);
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
                        IsKilled = true;
                    }
                }
            }
            else if (facingDirectionIndicator == true)
            {
                spriteBatch.Draw(_texture, Position, animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (facingDirectionIndicator == false)
            {
                spriteBatch.Draw(_texture, Position, animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }

            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
            spriteBatch.DrawRectangle(AdditionalHitBox_1, Color.Yellow);
            spriteBatch.DrawRectangle(DeathRectangle, Color.Red);
        }

    }
}
