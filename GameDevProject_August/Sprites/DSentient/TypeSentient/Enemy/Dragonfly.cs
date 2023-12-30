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
            : base(moveTexture, deathTexture, startPosition)
        {
            Movement.Direction = Direction.Up;

            RectangleHitbox = new Rectangle((int)startPosition.X, (int)startPosition.Y, 51, 39);
            hitboxes.Add("SoftSpot1", RectangleHitbox);

            numberOfCodeToFall = 2;

            // Spritesheet looks to the right
            #region MoveAnimation
            animationMove.AddFrame(new AnimationFrame(new Rectangle(0, 0, 51, 42)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(96, 0, 51, 42)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(192, 0, 51, 42)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(288, 0, 51, 42)));
            #endregion
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

            if (Movement.Direction == Direction.Up)
            {
                Velocity.Y -= Speed;
            }
            if (Movement.Direction == Direction.Down)
            {
                Velocity.Y += Speed;
            }
        }

        protected override void UniqueCollisionRules(Sprite sprite, Rectangle hitbox, bool isHardSpot)
        {

        }

        protected override void HitBoxTracker()
        {
            hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y, 51, 39);
        }

        protected override void UniqueDrawRules(SpriteBatch spriteBatch)
        {
            if (Movement.Direction == Direction.Up || Movement.Direction == Direction.Down)
            {
                animationHandlerEnemy.DrawAnimation(spriteBatch, animationMove, Position, Direction.Left);
            }

            spriteBatch.DrawRectangle(DeathRectangle, Color.Red);
            spriteBatch.DrawRectangle(hitboxes["SoftSpot1"], Color.Yellow);
        }
    }
}