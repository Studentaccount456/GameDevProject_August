using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy
{
    public class Dragonfly : Enemy
    {
        private bool isMovingUp = true;

        public Dragonfly(Texture2D moveTexture, Texture2D deathTexture, Vector2 startPosition)
            : base(moveTexture,deathTexture)
        {
            _texture = moveTexture;
            facingDirectionIndicator = false;

            RectangleHitbox = new Rectangle((int)startPosition.X, (int)startPosition.Y, 51, 39);

            hitboxes.Add("SoftSpot1",RectangleHitbox);

            // Standard walks right
            #region MoveAnimation
            animationMove = new Animation(AnimationType.Move, moveTexture);
            animationMove.fps = 8;
            animationMove.AddFrame(new AnimationFrame(new Rectangle(0, 0, 51, 42)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(96, 0, 51, 42)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(192, 0, 51, 42)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(288, 0, 51, 42)));
            #endregion
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            PositionTracker();

            Move(gameTime, blocks);

            CollisionRules(gameTime, sprites);

        }

        protected override void PositionTracker()
        {
            hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y, 51, 39);
        }

        protected override void UniqueCollisionRules(Sprite sprite, Rectangle hitbox, bool isHardSpot)
        {

        }

        protected override void UniqueMovingRules(GameTime gameTime, List<Block> blocks)
        {
            foreach (var block in blocks)
            {
                if (block.BlockRectangle.Intersects(hitboxes["SoftSpot1"]) && block.EnemyBehavior == true)
                {
                    isMovingUp = !isMovingUp;
                }
            }

            if (!isDeathAnimating)
            {
                if (isMovingUp)
                {
                    Velocity.Y -= Speed;
                }
                if (!isMovingUp)
                {
                    Velocity.Y += Speed;
                }
                animationMove.Update(gameTime);
            }
        }

        protected override void UniqueDrawRules(SpriteBatch spriteBatch)
        {
            if (isMovingUp || !isMovingUp)
            {
                spriteBatch.Draw(_texture, Position, animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }

            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
            spriteBatch.DrawRectangle(DeathRectangle, Color.Red);
            spriteBatch.DrawRectangle(hitboxes["SoftSpot1"], Color.Yellow);
        }
    }
}
