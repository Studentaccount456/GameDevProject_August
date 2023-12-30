using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Levels;
using GameDevProject_August.Models.Movement;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy.SpotterEnemy.ShootingEnemy.Enemies
{
    public class RatMage : ShootingEnemy
    {
        private Animation animationIdle;

        public Texture2D IdleTexture;

        private bool isIdling = false;
        private const float IdleTimeoutDuration = 5.0f;
        private float idleTimer = 0f;

        public RatMage(Texture2D moveTexture, Texture2D shootTexture, Texture2D idleTexture, Texture2D deathTexture,
            Vector2 startPosition, Vector2 offsetPositionSpotter, int widthSpotter, int heightSpotter)
            : base(moveTexture, deathTexture, shootTexture, startPosition, offsetPositionSpotter, widthSpotter, heightSpotter)
        {
            MoveTexture = moveTexture;
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
            animationShoot.fps = 6;
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(0, 0, 60, 48)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(96, 0, 51, 48)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(192, 0, 42, 48)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(288, 0, 42, 48)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(384, 0, 72, 48)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(480, 0, 69, 48)));
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

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            PositionTracker();

            InitializeEnemySpotter(Position, _offsetPositonSpotter, _widthSpotter, _heightSpotter);

            IdleFunctionality(gameTime);

            Move(gameTime, blocks);

            ShootingFunctionality(gameTime, sprites);

            CollisionRules(gameTime, sprites);

            UpdatePositionAndResetVelocity();
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

        private void IdleFunctionality(GameTime gameTime)
        {
            idleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (idleTimer >= IdleTimeoutDuration + 1f && !isShootingAnimating)
            {
                isIdling = false;
                idleTimer = 0;
            }
            else if (idleTimer >= IdleTimeoutDuration && !isShootingAnimating)
            {
                isIdling = true;
                animationIdle.Update(gameTime);
            }
        }


        protected override void PositionTracker()
        {
            hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y, 60, 49);
            // Necessary When not override Rectanglehitbox with getter
        }

        protected override void UniqueMovingRules(GameTime gameTime, List<Block> blocks)
        {
            if (!isIdling && !isShootingAnimating)
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

        protected override void UniqueDrawRules(SpriteBatch spriteBatch)
        {
            if (isShootingAnimating)
            {
                if (Movement.Direction == Direction.Right)
                {
                    spriteBatch.Draw(ShootTexture, Position, animationShoot.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
                }
                else if (Movement.Direction == Direction.Left)
                {
                    spriteBatch.Draw(ShootTexture, Position, animationShoot.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
                }
            }
            else if (isIdling)
            {
                if (Movement.Direction == Direction.Right)
                {
                    spriteBatch.Draw(IdleTexture, Position, animationIdle.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
                }
                else if (Movement.Direction == Direction.Left)
                {
                    spriteBatch.Draw(IdleTexture, Position, animationIdle.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
                }
            }
            else if (Movement.Direction == Direction.Right && !isIdling)
            {
                spriteBatch.Draw(MoveTexture, Position + new Vector2(0, -4), animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (Movement.Direction == Direction.Left && !isIdling)
            {
                spriteBatch.Draw(MoveTexture, Position + new Vector2(0, -4), animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }

            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
            spriteBatch.DrawRectangle(DeathRectangle, Color.Red);
            spriteBatch.DrawRectangle(EnemySpotter, Color.Red);
            spriteBatch.DrawRectangle(hitboxes["SoftSpot1"], Color.Yellow);
        }
    }
}
