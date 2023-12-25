using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Levels;
using GameDevProject_August.Models.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy
{
    public class Dragonfly : Enemy
    {
        public Dragonfly(Texture2D moveTexture, Texture2D deathTexture, Vector2 startPosition)
            : base(moveTexture,deathTexture)
        {
            MoveTexture = moveTexture;
            facingDirectionIndicator = false;
            numberOfCodeToFall = 2;

            Movement.Direction = Direction.Up;

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
                    Movement.flipDirectionUpAndDown();
                } 
            }

            if (!isDeathAnimating)
            {
                if (Movement.Direction == Direction.Up)
                {
                    Velocity.Y -= Speed;
                }
                if (Movement.Direction == Direction.Down)
                {
                    Velocity.Y += Speed;
                }
                animationMove.Update(gameTime);
            }
        }

        protected override void UniqueDrawRules(SpriteBatch spriteBatch)
        {
            if (Movement.Direction == Direction.Up || Movement.Direction == Direction.Down)
            {
                spriteBatch.Draw(MoveTexture, Position, animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }

            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
            spriteBatch.DrawRectangle(DeathRectangle, Color.Red);
            spriteBatch.DrawRectangle(hitboxes["SoftSpot1"], Color.Yellow);
        }
    }
}
