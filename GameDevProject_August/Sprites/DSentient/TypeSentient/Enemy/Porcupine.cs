using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Levels;
using GameDevProject_August.Sprites.DNotSentient;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy
{
    public class Porcupine : Enemy
    {
        private Animation animationMove;

        public Rectangle AdditionalHitBox_1;

        public Porcupine(Texture2D moveTexture, Texture2D deathTexture)
            : base(moveTexture, deathTexture)
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

                if ((sprite.RectangleHitbox.Intersects(RectangleHitbox) || sprite.RectangleHitbox.Intersects(AdditionalHitBox_1)) && sprite is Archeologist && sprite is Sentient sentient)
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
