using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Levels;
using GameDevProject_August.Models.Movement;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player.TypeOfPlayer.ShootingPlayer.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy.AttackingEnemy.SpotterEnemy.MeleeEnemy.Enemies
{
    public class Minotaur : MeleeEnemy
    {
        public Minotaur(Texture2D moveTexture, Texture2D shootTexture, Texture2D idleTexture, Texture2D deathTexture,
                        Vector2 startPosition, Vector2 offsetPositionSpotter, int widthSpotter, int heightSpotter)
            : base(moveTexture, shootTexture, deathTexture, startPosition, offsetPositionSpotter, widthSpotter, heightSpotter)
        {
            offsetAnimationAttack = new Vector2(0, -30);

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
            #region animationShoot
            animationAttack.fps = 2;
            animationAttack.AddFrame(new AnimationFrame(new Rectangle(0, 0, 63, 72)));
            animationAttack.AddFrame(new AnimationFrame(new Rectangle(96, 0, 69, 72)));
            animationAttack.AddFrame(new AnimationFrame(new Rectangle(192, 0, 75, 72)));
            animationAttack.AddFrame(new AnimationFrame(new Rectangle(288, 0, 75, 72)));
            animationAttack.AddFrame(new AnimationFrame(new Rectangle(384, 0, 72, 72)));
            animationAttack.AddFrame(new AnimationFrame(new Rectangle(480, 0, 72, 72)));
            animationAttack.AddFrame(new AnimationFrame(new Rectangle(576, 0, 75, 72)));
            animationAttack.AddFrame(new AnimationFrame(new Rectangle(672, 0, 72, 72)));
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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isIdling && !isAttackingAnimating)
            {
                animationHandlerEnemy.DrawAnimation(spriteBatch, animationIdle, Position, Direction.Left);
            }
            else
            {
                base.Draw(spriteBatch);
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

            if (idleTimer >= IdleTimeoutDuration && !isAttackingAnimating && isIdling && !isAttackCooldown)
            {
                isAttackingAnimating = true;
                isIdling = false;
                idleTimer = 0;
            }
            if (isIdling)
            {
                animationIdle.Update(gameTime);
            }

            if (Movement.Direction == Direction.Right || Movement.Direction == Direction.Left && !isAttackingAnimating)
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

            if (isAttackingAnimating)
            {
                hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y - 15, 63, 42);
                hitboxes["SoftSpot2"] = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
                meleeAttackAnimationFrameIndex = animationAttack.CurrentFrameIndex;
                canSeeEnemy = false;
                if (meleeAttackAnimationFrameIndex == 1)
                {
                    hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y, 69, 42);
                }
                else if (meleeAttackAnimationFrameIndex == 2 || meleeAttackAnimationFrameIndex == 3 || meleeAttackAnimationFrameIndex == 6)
                {
                    hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y, 75, 42);
                }
                else if (meleeAttackAnimationFrameIndex == 4)
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
                else if (meleeAttackAnimationFrameIndex == 5)
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
                else if (meleeAttackAnimationFrameIndex == 7)
                {
                    hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y - 15, 72, 42);
                    idleTimer = 0;
                    isAttackingAnimating = false;
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
