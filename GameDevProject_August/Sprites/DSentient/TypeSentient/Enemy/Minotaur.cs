using GameDevProject_August.AnimationClasses;
using GameDevProject_August.AnimationClasses.AnimationMethods;
using GameDevProject_August.Levels;
using GameDevProject_August.Sprites.DNotSentient;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles;
using GameDevProject_August.Sprites.DSentient;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy
{
    public class Minotaur : Sentient
    {
        private Animation animationMove;
        private Animation animationDeath;
        private Animation animationIdle;
        private Animation animationShoot;

        public Texture2D ShootTexture;
        public Texture2D IdleTexture;
        public Texture2D DeathTexture;

        private bool isShootingAnimating = false;

        private bool isShootingCooldown = false;
        private const float ShootingCooldownDuration = 1f;
        private float shootingCooldownTimer = 0f;

        private bool isIdling = false;
        private const float IdleTimeoutDuration = 5.0f;
        private float idleTimer = 0f;

        private int deathAnimationFrameIndex = 0;
        private int shootAnimationFrameIndex = 0;


        private Vector2 OffsetAnimation;

        private bool reachedFourthDeathFrame = false;

        // Tongue Rectangle
        public Rectangle AdditionalHitBox_1;

        public Rectangle DeathRectangle;

        public AnimationHandler AnimationHandler_Minotaur;

        private bool canSeeEnemy = true;

        private bool _enemySpotted;

        public Rectangle EnemySpotter;

        public Vector2 EnemyPosition;

        private int _widthSpotter, _heightSpotter;
        private Vector2 _offsetPositonSpotter;

        public Minotaur(Texture2D moveTexture, Texture2D shootTexture, Texture2D idleTexture, Texture2D deathTexture,
                        Vector2 startPosition, Vector2 offsetPositionSpotter, int widthSpotter, int heightSpotter)
            : base(moveTexture)
        {
            _texture = moveTexture;
            ShootTexture = shootTexture;
            IdleTexture = idleTexture;
            DeathTexture = deathTexture;

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

            #region Death
            animationDeath = new Animation(AnimationType.Death, deathTexture);
            animationDeath.fps = 4;
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(0, 0, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(64, 0, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(128, 0, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(192, 0, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(0, 64, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(64, 64, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(128, 64, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(192, 64, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(0, 128, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(64, 128, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(128, 128, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(192, 128, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(0, 192, 64, 64)));
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

        private void PositionTracker()
        {
            // Necessary When not override Rectanglehitbox with getter
            PositionXRectangleHitbox = (int)Position.X;
            PositionYRectangleHitbox = (int)Position.Y;
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

        private void CollisionRules(GameTime gameTime, List<Sprite> sprites)
        {
            if (facingDirectionIndicator || !facingDirectionIndicator && !isShootingAnimating)
            {
                WidthRectangleHitbox = 54;
                HeightRectangleHitbox = 49;
                PositionYRectangleHitbox = (int)Position.Y - 5;
            }

            if (isIdling)
            {
                WidthRectangleHitbox = 63;
                HeightRectangleHitbox = 44;
            }


            foreach (var sprite in sprites)
            {
                if (sprite is Minotaur)
                {
                    continue;
                }

                if ((sprite.RectangleHitbox.Intersects(RectangleHitbox) || sprite.RectangleHitbox.Intersects(AdditionalHitBox_1)) && sprite is MainCharacter && sprite is Sentient sentient)
                {
                    sentient.isDeathAnimating = true;
                }

                GlitchDeathInit(gameTime, sprite, 1);

                if (sprite.RectangleHitbox.Intersects(EnemySpotter) && sprite is MainCharacter && canSeeEnemy)
                {
                    _enemySpotted = true;
                    isIdling = false;
                }
                if ((sprite.RectangleHitbox.Intersects(RectangleHitbox) || sprite.RectangleHitbox.Intersects(AdditionalHitBox_1)) && sprite is PlayerBullet && sprite is NotSentient notSentient)
                {
                    Game1.PlayerScore.MainScore++;
                    isDeathAnimating = true;
                    notSentient.IsDestroyed = true;
                }
            }
        }

        private void GlitchDeathInit(GameTime gameTime, Sprite sprite, int pieceOfCodeToFall)
        {
            if (isDeathAnimating)
            {
                DeathRectangle = new Rectangle((int)Position.X, (int)Position.Y, 64, 64);
                WidthRectangleHitbox = 0;
                HeightRectangleHitbox = 0;

                if (sprite.RectangleHitbox.Intersects(DeathRectangle) && sprite is MainCharacter && sprite is Sentient sentient)
                {
                    sentient.isDeathAnimating = true;
                }

                animationDeath.Update(gameTime);

                deathAnimationFrameIndex = animationDeath.CurrentFrameIndex;

                if (deathAnimationFrameIndex == 3) // 4th frame
                {
                    reachedFourthDeathFrame = true;
                }

                if (reachedFourthDeathFrame && animationDeath.IsAnimationComplete)
                {
                    PieceOfCodeToFall = pieceOfCodeToFall;
                    IsKilled = true;
                }

                if (deathAnimationFrameIndex > 6)
                {
                    DeathRectangle.Width = 0;
                    DeathRectangle.Height = 0;
                }

            }
        }

        private void UpdatePositionAndResetVelocity()
        {
            Position += Velocity;

            Velocity = Vector2.Zero;
        }

        private void MinotaurAttack(GameTime gameTime)
        {
            animationShoot.Update(gameTime);
            if (isShootingAnimating)
            {
                PositionXRectangleHitbox = (int)Position.X;
                PositionYRectangleHitbox = (int)Position.Y;
                WidthRectangleHitbox = 63;
                HeightRectangleHitbox = 42;
                AdditionalHitBox_1.Width = 0;
                AdditionalHitBox_1.Height = 0;
                shootAnimationFrameIndex = animationShoot.CurrentFrameIndex;
                canSeeEnemy = false;
                if (shootAnimationFrameIndex == 1)
                {
                    WidthRectangleHitbox = 69;
                    HeightRectangleHitbox = 42;
                }
                else if (shootAnimationFrameIndex == 2 || shootAnimationFrameIndex == 3 || shootAnimationFrameIndex == 6)
                {
                    WidthRectangleHitbox = 75;
                    HeightRectangleHitbox = 42;
                }
                else if (shootAnimationFrameIndex == 4)
                {
                    WidthRectangleHitbox = 54;
                    HeightRectangleHitbox = 42;
                    //Tongue
                    if (facingDirectionIndicator)
                    {
                        AdditionalHitBox_1.X = (int)Position.X + 45;
                    }
                    else
                    {
                        AdditionalHitBox_1.X = (int)Position.X;
                    }
                    AdditionalHitBox_1.Y = (int)Position.Y - 30;
                    AdditionalHitBox_1.Width = 27;
                    AdditionalHitBox_1.Height = 30;
                }
                else if (shootAnimationFrameIndex == 5)
                {
                    WidthRectangleHitbox = 69;
                    HeightRectangleHitbox = 42;
                    //Tongue
                    if (facingDirectionIndicator)
                    {
                        AdditionalHitBox_1.X = (int)Position.X + 63;
                    }
                    else
                    {
                        AdditionalHitBox_1.X = (int)Position.X;
                    }
                    AdditionalHitBox_1.Y = (int)Position.Y - 15;
                    AdditionalHitBox_1.Width = 9;
                    AdditionalHitBox_1.Height = 15;
                }
                else if (shootAnimationFrameIndex == 7)
                {
                    WidthRectangleHitbox = 72;
                    HeightRectangleHitbox = 42;
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
                    if (IsTouchingLeftBlock(block) && block.EnemyBehavior == true)
                    {
                        facingDirectionIndicator = false;
                    }
                    else if (IsTouchingRightBlock(block) && block.EnemyBehavior == true)
                    {
                        facingDirectionIndicator = true;
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



        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDeathAnimating)
            {
                if (animationDeath.IsAnimationComplete)
                {
                    IsKilled = true;
                }
                AnimationHandler_Minotaur.DrawAnimation(spriteBatch, animationDeath, Position, true);
            }
            else if (isIdling)
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
        }
    }
}
