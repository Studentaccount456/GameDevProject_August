using GameDevProject_August.AnimationClasses;
using GameDevProject_August.AnimationClasses.AnimationMethods;
using GameDevProject_August.Levels;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy
{
    public class Minotaur : Enemy
    {
        private Animation animationMove;
        private Animation animationIdle;
        private Animation animationShoot;

        public Texture2D ShootTexture;
        public Texture2D IdleTexture;

        private bool isShootingAnimating = false;

        private bool isShootingCooldown = false;
        private const float ShootingCooldownDuration = 1f;
        private float shootingCooldownTimer = 0f;

        private bool isIdling = false;
        private const float IdleTimeoutDuration = 5.0f;
        private float idleTimer = 0f;

        private int shootAnimationFrameIndex = 0;


        private Vector2 OffsetAnimation;

        public AnimationHandler AnimationHandler_Minotaur;

        private bool canSeeEnemy = true;

        private bool _enemySpotted;

        public Rectangle EnemySpotter;

        public Vector2 EnemyPosition;

        private int _widthSpotter, _heightSpotter;
        private Vector2 _offsetPositonSpotter;

        public Minotaur(Texture2D moveTexture, Texture2D shootTexture, Texture2D idleTexture, Texture2D deathTexture,
                        Vector2 startPosition, Vector2 offsetPositionSpotter, int widthSpotter, int heightSpotter)
            : base(moveTexture,deathTexture)
        {
            _texture = moveTexture;
            ShootTexture = shootTexture;
            IdleTexture = idleTexture;

            hitboxes.Add("SoftSpot1", RectangleHitbox);
            hitboxes.Add("SoftSpot2", AdditionalHitBox_1);

            AnimationHandler_Minotaur = new AnimationHandler();

            AdditionalHitBox_1 = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);

            facingDirectionIndicator = false;

            isIdling = true;

            _offsetPositonSpotter = offsetPositionSpotter;
            _widthSpotter = widthSpotter;
            _heightSpotter = heightSpotter;

            Position = startPosition;

            InitializeEnemySpotter(Position, _offsetPositonSpotter, _widthSpotter, _heightSpotter);


            #region MoveAnimation
            animationMove = new Animation(AnimationType.Move, moveTexture);
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
            animationShoot = new Animation(AnimationType.Attack, shootTexture);
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

        private void InitializeEnemySpotter(Vector2 position, Vector2 offsetPositionSpotter, int widthSpotter, int heightSpotter)
        {
            // Initialize in Constructor + use RemoveEnemySpotterSpotted() when spotter needs dissapear after spot
            EnemySpotter = new Rectangle((int)(position.X - offsetPositionSpotter.X), (int)(position.Y - offsetPositionSpotter.Y), widthSpotter, heightSpotter);
            // Otherwise put in update so the Position updates so the spot can be reset
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            PositionTracker();

            IdleFunctionality(gameTime);

            RemoveEnemySpotterSpotted(_enemySpotted);

            ShootCooldown(gameTime);

            MinotaurAttack(gameTime);

            Move(gameTime, blocks);

            CollisionRules(gameTime, sprites);

            UpdatePositionAndResetVelocity();
        }

        protected override void PositionTracker()
        {
            hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y, 63, 44);
            // Necessary When not override Rectanglehitbox with getter
        }

        private void IdleFunctionality(GameTime gameTime)
        {
            idleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (idleTimer >= IdleTimeoutDuration && !isShootingAnimating && isIdling && !isShootingCooldown)
            {
                isShootingAnimating = true;
                isIdling = false;
                idleTimer = 0;
            }
            if (isIdling)
            {
                animationIdle.Update(gameTime);
            }

            if (facingDirectionIndicator || !facingDirectionIndicator && !isShootingAnimating)
            {
                hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y, 54, 49);
            }

            if (isIdling)
            {
                hitboxes["SoftSpot1"] = new Rectangle((int)Position.X, (int)Position.Y, 63, 44);
            }
        }

        private void ShootCooldown(GameTime gameTime)
        {
            // Shooting cooldown
            if (isShootingCooldown)
            {
                shootingCooldownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (shootingCooldownTimer >= ShootingCooldownDuration)
                {
                    isShootingCooldown = false;
                }
            }
        }

        protected override void SpecificCollisionRules(Sprite sprite, Rectangle hitbox, bool isHardSpot)
        {
            if (sprite.RectangleHitbox.Intersects(EnemySpotter) && sprite is Archeologist && canSeeEnemy)
            {
                _enemySpotted = true;
                isIdling = false;
            }          
        }

        private void MinotaurAttack(GameTime gameTime)
        {
            animationShoot.Update(gameTime);
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
                    if (facingDirectionIndicator)
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
                    if (facingDirectionIndicator)
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

        private void RemoveEnemySpotterSpotted(bool enemySpotted)
        {
            if (enemySpotted)
            {
                EnemySpotter = Rectangle.Empty;
            }
        }

        private void Move(GameTime gameTime, List<Block> blocks)
        {
            if (!isDeathAnimating && _enemySpotted && canSeeEnemy)
            {
                foreach (var block in blocks)
                {
                    if (block.BlockRectangle.Intersects(hitboxes["SoftSpot1"]) && block.EnemyBehavior == true)
                    {
                        facingDirectionIndicator = !facingDirectionIndicator;
                    }
                }

                if (!facingDirectionIndicator)
                {
                    Velocity.X -= Speed;
                    facingDirection = -Vector2.UnitX;
                }
                if (facingDirectionIndicator)
                {
                    Velocity.X += Speed;
                    facingDirection = Vector2.UnitX;
                }

                Position = Vector2.Clamp(Position, new Vector2(0 - RectangleHitbox.Width, 0 + RectangleHitbox.Height / 2), new Vector2(Game1.ScreenWidth - RectangleHitbox.Width, Game1.ScreenHeight - RectangleHitbox.Height / 2));
                animationMove.Update(gameTime);
            }
        }

        protected override void UniqueDrawRules(SpriteBatch spriteBatch)
        {
            if (isIdling)
            {
                spriteBatch.Draw(IdleTexture, Position, animationIdle.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }
            else if (isShootingAnimating)
            {
                if (facingDirectionIndicator == true)
                {
                    spriteBatch.Draw(ShootTexture, Position + OffsetAnimation, animationShoot.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
                }
                else if (facingDirectionIndicator == false)
                {
                    spriteBatch.Draw(ShootTexture, Position + OffsetAnimation, animationShoot.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
                }
            }
            else if (facingDirectionIndicator)
            {
                spriteBatch.Draw(_texture, Position + new Vector2(0, -8), animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (!facingDirectionIndicator)
            {
                spriteBatch.Draw(_texture, Position + new Vector2(0, -8), animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }

            spriteBatch.DrawRectangle(AdditionalHitBox_1, Color.Blue);
            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
            spriteBatch.DrawRectangle(DeathRectangle, Color.Red);
            spriteBatch.DrawRectangle(EnemySpotter, Color.Yellow);
            spriteBatch.DrawRectangle(hitboxes["SoftSpot1"], Color.Black);
            spriteBatch.DrawRectangle(hitboxes["SoftSpot2"], Color.White);
        }
    }
}
