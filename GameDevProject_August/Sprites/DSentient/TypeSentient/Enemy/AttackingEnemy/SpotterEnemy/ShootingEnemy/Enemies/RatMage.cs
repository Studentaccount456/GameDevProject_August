using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Levels;
using GameDevProject_August.Models.Movement;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy.AttackingEnemy.SpotterEnemy.ShootingEnemy.Enemies
{
    public class RatMage : ShootingEnemy
    {
        public RatMage(Texture2D moveTexture, Texture2D shootTexture, Texture2D idleTexture, Texture2D deathTexture,
            Vector2 startPosition, Vector2 offsetPositionSpotter, int widthSpotter, int heightSpotter)
            : base(moveTexture, deathTexture, shootTexture, startPosition, offsetPositionSpotter, widthSpotter, heightSpotter)
        {
            offsetMoveAnimation = new Vector2(0, -4);

            IdleTexture = idleTexture;

            numberOfCodeToFall = 4;

            hitboxes.Add("SoftSpot1", RectangleHitbox);

            // Standard walks right
            #region MoveAnimation
            animationMove.fps = 12;
            animationMove.AddFrame(new AnimationFrame(new Rectangle(0, 0, 60, 57)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(96, 0, 57, 57)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(192, 0, 54, 57)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(288, 0, 57, 57)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(384, 0, 60, 57)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(480, 0, 60, 57)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(579, 0, 57, 57)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(672, 0, 60, 57)));
            #endregion

            //Height is 48 for each frame
            #region animationCast
            animationAttack.fps = 6;
            animationAttack.AddFrame(new AnimationFrame(new Rectangle(0, 0, 60, 48)));
            animationAttack.AddFrame(new AnimationFrame(new Rectangle(96, 0, 51, 48)));
            animationAttack.AddFrame(new AnimationFrame(new Rectangle(192, 0, 42, 48)));
            animationAttack.AddFrame(new AnimationFrame(new Rectangle(288, 0, 42, 48)));
            animationAttack.AddFrame(new AnimationFrame(new Rectangle(384, 0, 72, 48)));
            animationAttack.AddFrame(new AnimationFrame(new Rectangle(480, 0, 69, 48)));
            #endregion

            //Height is 44 for each frame
            #region Idle
            animationIdle = new Animation(AnimationType.Idle, idleTexture);
            animationIdle.fps = 10;
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(0, 0, 60, 48)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(99, 0, 57, 48)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(195, 0, 57, 48)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(288, 0, 60, 48)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(384, 0, 60, 48)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(483, 0, 57, 48)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(579, 0, 57, 48)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(672, 0, 60, 48)));
            #endregion
        }

        protected override void UniqueMovingRules(GameTime gameTime, List<Block> blocks)
        {
            if (!isIdling && !isAttackingAnimating)
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
                if (Movement.Direction == Direction.Right)
                {
                    Velocity.X += Speed;
                    facingDirection = Vector2.UnitX;
                }
            }
        }

        protected override void UniqueCollisionRules(Sprite sprite, Rectangle hitbox, bool isHardSpot)
        {
            if (sprite.RectangleHitbox.Intersects(EnemySpotter) && sprite is Archeologist)
            {
                enemySpotted = true;
                EnemyPosition.X = sprite.Position.X;
            }
            if (!sprite.RectangleHitbox.Intersects(EnemySpotter) && sprite is Archeologist)
            {
                enemySpotted = false;
            }

        }

        protected override void HitBoxTracker()
        {
            hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y, 60, 49);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isIdling && !isAttackingAnimating)
            {
                if (Movement.Direction == Direction.Right)
                {
                    animationHandlerEnemy.DrawAnimation(spriteBatch, animationIdle, Position, Direction.Right);
                }
                else if (Movement.Direction == Direction.Left)
                {
                    animationHandlerEnemy.DrawAnimation(spriteBatch, animationIdle, Position, Direction.Left);
                }
            }
            else
            {
                base.Draw(spriteBatch);
            }

            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
            spriteBatch.DrawRectangle(DeathRectangle, Color.Red);
            spriteBatch.DrawRectangle(EnemySpotter, Color.Red);
            spriteBatch.DrawRectangle(hitboxes["SoftSpot1"], Color.Yellow);
        }

        protected override void IdleFunctionality(GameTime gameTime)
        {
            idleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (idleTimer >= IdleTimeoutDuration + 1f && !isAttackingAnimating)
            {
                isIdling = false;
                idleTimer = 0;
            }
            else if (idleTimer >= IdleTimeoutDuration && !isAttackingAnimating)
            {
                isIdling = true;
                animationIdle.Update(gameTime);
            }
        }
    }
}
