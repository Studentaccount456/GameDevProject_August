using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Levels;
using GameDevProject_August.Models.Movement;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy.SpotterEnemy.MeleeEnemy.Enemies
{
    public class Minotaur : MeleeEnemy
    {
        private Vector2 OffsetAnimation;

        public Minotaur(Texture2D moveTexture, Texture2D shootTexture, Texture2D idleTexture, Texture2D deathTexture,
                        Vector2 startPosition, Vector2 offsetPositionSpotter, int widthSpotter, int heightSpotter)
            : base(moveTexture, shootTexture, deathTexture, startPosition, offsetPositionSpotter, widthSpotter, heightSpotter)
        {
            IdleTexture = idleTexture;
            numberOfCodeToFall = 1;

            hitboxes.Add("SoftSpot1", RectangleHitbox);
            hitboxes.Add("SoftSpot2", AdditionalHitBox_1);

            AdditionalHitBox_1 = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);

            Movement.Direction = Direction.Left;

            isIdling = true;

            #region MoveAnimation
            animationMove.fps = 12;
            animationMove.AddFrame(new AnimationFrame(new Rectangle(0, 0, 54, 51)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(96, 0, 54, 51)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(192, 0, 54, 51)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(288, 0, 54, 51)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(384, 0, 54, 51)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(480, 0, 54, 51)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(576, 0, 54, 51)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(672, 0, 54, 51)));
            #endregion

            // Height for standstill without attack is 42
            OffsetAnimation = new Vector2(0, -30);
            #region animationShoot
            animationShoot.fps = 2;
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(0, 0, 63, 72)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(96, 0, 69, 72)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(192, 0, 75, 72)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(288, 0, 75, 72)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(384, 0, 72, 72)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(480, 0, 72, 72)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(576, 0, 75, 72)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(672, 0, 72, 72)));
            #endregion

            #region Idle
            animationIdle = new Animation(AnimationType.Idle, idleTexture);
            animationIdle.fps = 8;
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(0, 0, 63, 42)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(96, 0, 63, 42)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(192, 0, 63, 42)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(288, 0, 66, 42)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(384, 0, 66, 42)));
            #endregion

        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            base.Update(gameTime, sprites, blocks);

            RemoveEnemySpotterSpotted(_enemySpotted);
        }

        protected override void UniqueMovingRules(GameTime gameTime, List<Block> blocks)
        {
            if (_enemySpotted && canSeeEnemy)
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
            if (sprite.RectangleHitbox.Intersects(EnemySpotter) && sprite is Archeologist && canSeeEnemy)
            {
                _enemySpotted = true;
                isIdling = false;
            }
        }

        protected override void HitBoxTracker()
        {
            hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y, 63, 44);
            // Necessary When not override Rectanglehitbox with getter
        }

        protected override void UniqueDrawRules(SpriteBatch spriteBatch)
        {
            if (isIdling)
            {
                spriteBatch.Draw(IdleTexture, Position, animationIdle.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }
            else if (isShootingAnimating)
            {
                if (Movement.Direction == Direction.Right)
                {
                    spriteBatch.Draw(ShootTexture, Position + OffsetAnimation, animationShoot.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
                }
                else if (Movement.Direction == Direction.Left)
                {
                    spriteBatch.Draw(ShootTexture, Position + OffsetAnimation, animationShoot.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
                }
            }
            else if (Movement.Direction == Direction.Right)
            {
                spriteBatch.Draw(MoveTexture, Position + new Vector2(0, -8), animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (Movement.Direction == Direction.Left)
            {
                spriteBatch.Draw(MoveTexture, Position + new Vector2(0, -8), animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }

            spriteBatch.DrawRectangle(AdditionalHitBox_1, Color.Blue);
            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
            spriteBatch.DrawRectangle(DeathRectangle, Color.Red);
            spriteBatch.DrawRectangle(EnemySpotter, Color.Yellow);
            spriteBatch.DrawRectangle(hitboxes["SoftSpot1"], Color.Black);
            spriteBatch.DrawRectangle(hitboxes["SoftSpot2"], Color.White);
        }


        protected override void IdleFunctionality(GameTime gameTime)
        {
            idleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (idleTimer >= IdleTimeoutDuration && !isShootingAnimating && isIdling && !isAttackCooldown)
            {
                isShootingAnimating = true;
                isIdling = false;
                idleTimer = 0;
            }
            if (isIdling)
            {
                animationIdle.Update(gameTime);
            }

            if (Movement.Direction == Direction.Right || Movement.Direction == Direction.Left && !isShootingAnimating)
            {
                hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y, 54, 49);
            }

            if (isIdling)
            {
                hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y, 63, 44);
            }
        }

        protected override void UniqueMeleeAttackImplementation(GameTime gameTime)
        {
            AttackCooldown(gameTime);

            if (isShootingAnimating)
            {
                hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y - 15, 63, 42);
                hitboxes["SoftSpot2"] = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
                shootAnimationFrameIndex = animationShoot.CurrentFrameIndex;
                canSeeEnemy = false;
                if (shootAnimationFrameIndex == 1)
                {
                    hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y, 69, 42);
                }
                else if (shootAnimationFrameIndex == 2 || shootAnimationFrameIndex == 3 || shootAnimationFrameIndex == 6)
                {
                    hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y, 75, 42);
                }
                else if (shootAnimationFrameIndex == 4)
                {
                    hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y, 76, 42);
                    //Tongue
                    if (Movement.Direction == Direction.Right)
                    {
                        hitboxes["SoftSpot2"] = new Rectangle((int)Position.X + 45, (int)Position.Y - 30, 27, 30);
                    }
                    else
                    {
                        hitboxes["SoftSpot2"] = new Rectangle((int)Position.X, (int)Position.Y - 30, 27, 30);
                    }
                }
                else if (shootAnimationFrameIndex == 5)
                {
                    hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y, 69, 42);
                    //Tongue
                    if (Movement.Direction == Direction.Right)
                    {
                        hitboxes["SoftSpot2"] = new Rectangle((int)Position.X + 63, (int)Position.Y - 15, 9, 15);
                    }
                    else
                    {
                        hitboxes["SoftSpot2"] = new Rectangle((int)Position.X, (int)Position.Y - 15, 9, 15);
                    }
                }
                else if (shootAnimationFrameIndex == 7)
                {
                    hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y - 15, 72, 42);
                    idleTimer = 0;
                    isShootingAnimating = false;
                    isIdling = true;
                    canSeeEnemy = true;
                }
            }
        }

        protected void RemoveEnemySpotterSpotted(bool enemySpotted)
        {
            if (enemySpotted)
            {
                EnemySpotter = Rectangle.Empty;
            }
        }
    }
}
