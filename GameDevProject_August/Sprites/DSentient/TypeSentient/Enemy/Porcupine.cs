using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Levels;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy
{
    public class Porcupine : Enemy
    {

        public Porcupine(Texture2D moveTexture, Texture2D deathTexture)
            : base(moveTexture, deathTexture)
        {
            MoveTexture = moveTexture;

            hitboxes.Add("SoftSpot1", RectangleHitbox);
            hitboxes.Add("HardSpot1", AdditionalHitBox_1);

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
            PositionTracker();
            Move(gameTime, blocks);
            PorcupineHitBoxFunct();

            CollisionRules(gameTime, sprites);

            UpdatePositionAndResetVelocity();
        }

        protected override void UniqueMovingRules(GameTime gameTime, List<Block> blocks)
        {
        foreach (var block in blocks)
        {
            if (block.BlockRectangle.Intersects(hitboxes["SoftSpot1"]) && block.EnemyBehavior == true)
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
        }
            animationMove.Update(gameTime);
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
            hitboxes["SoftSpot1"] = new Rectangle(rect2X, (int)Position.Y + 24, 15, 24);
            hitboxes["HardSpot1"] = new Rectangle(rect3X, (int)Position.Y, 42, 48);
        }

        protected override void UniqueCollisionRules(Sprite sprite, Rectangle hitbox, bool isHardSpot)
        {
            if (sprite.RectangleHitbox.Intersects(hitbox) && sprite is PlayerBullet playerbullet && isHardSpot == true)
            {
                playerbullet.IsDestroyed = true;
            }
        }

        protected override void UniqueDrawRules(SpriteBatch spriteBatch)
        {
            if (facingDirectionIndicator == true)
            {
                spriteBatch.Draw(MoveTexture, Position, animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (facingDirectionIndicator == false)
            {
                spriteBatch.Draw(MoveTexture, Position, animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }

            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
            spriteBatch.DrawRectangle(AdditionalHitBox_1, Color.Yellow);
            spriteBatch.DrawRectangle(DeathRectangle, Color.Red);
            spriteBatch.DrawRectangle(hitboxes["SoftSpot1"], Color.Black);
            spriteBatch.DrawRectangle(hitboxes["HardSpot1"], Color.White);
        }
    }
}
