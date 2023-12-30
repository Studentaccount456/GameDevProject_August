using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Levels;
using GameDevProject_August.Models.Movement;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles.Bullets.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy.PassiveEnemy
{
    public class Porcupine : Enemy
    {
        public Porcupine(Texture2D moveTexture, Texture2D deathTexture, Vector2 startPosition)
            : base(moveTexture, deathTexture, startPosition)
        {
            hitboxes.Add("SoftSpot1", RectangleHitbox);
            hitboxes.Add("HardSpot1", AdditionalHitBox_1);

            numberOfCodeToFall = 3;

            // Spritesheet looks to the right
            animationMove.AddConsistentFrames(96, 0, 57, 48, 5);
        }

        protected override void UniqueMovingRules(GameTime gameTime, List<Block> blocks)
        {
            foreach (var block in blocks)
            {
                if (block.BlockRectangle.Intersects(hitboxes["SoftSpot1"]) && block.EnemyBehavior == true)
                {
                    Movement.flipDirectionLeftAndRight();
                }
            }

            if (Movement.Direction == Direction.Left)
            {
                Velocity.X -= Speed;
                facingDirection = -Vector2.UnitX;
            }
            else if (Movement.Direction == Direction.Right)
            {
                Velocity.X += Speed;
                facingDirection = Vector2.UnitX;
            }
        }

        protected override void UniqueCollisionRules(Sprite sprite, Rectangle hitbox, bool isHardSpot)
        {
            if (sprite.RectangleHitbox.Intersects(hitbox) && sprite is PlayerBullet playerbullet && isHardSpot == true)
            {
                playerbullet.IsDestroyed = true;
            }
        }

        protected override void HitBoxTracker()
        {
            // Update Hitboxes when facingDirection Changes
            int rect2X = (int)Position.X + 42;
            int rect3X = (int)Position.X;

            if (Movement.Direction == Direction.Left)
            {
                // Left-Facing direction 
                rect2X -= 42;
                rect3X += 16;
            }

            // Update Hitboxes
            hitboxes["SoftSpot1"] = new Rectangle(rect2X, (int)Position.Y + 24, 15, 24);
            hitboxes["HardSpot1"] = new Rectangle(rect3X, (int)Position.Y, 42, 48);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
            spriteBatch.DrawRectangle(AdditionalHitBox_1, Color.Yellow);
            spriteBatch.DrawRectangle(DeathRectangle, Color.Red);
            spriteBatch.DrawRectangle(hitboxes["SoftSpot1"], Color.Black);
            spriteBatch.DrawRectangle(hitboxes["HardSpot1"], Color.White);
        }
    }
}
