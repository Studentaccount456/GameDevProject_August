using GameDevProject_August.AnimationClasses;
using GameDevProject_August.AnimationClasses.AnimationMethods;
using GameDevProject_August.Levels;
using GameDevProject_August.Sprites.NotSentient.Collectibles;
using GameDevProject_August.Sprites.NotSentient.Projectiles;
using GameDevProject_August.Sprites.Sentient.Characters.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.Sentient.Characters.Enemy
{
    public class MinotaurFix : Sprite
    {
        public bool HasDied = false;

        private Animation animationMove;
        private Animation animationDeath;
        private Animation animationIdle;
        private Animation animationShoot;

        public Texture2D ShootTexture;
        public Texture2D IdleTexture;
        public Texture2D DeathTexture;

        private bool canMove = true;
        private bool isMoving = false;

        private bool isDeathAnimating = false;
        private bool isShootingAnimating = false;

        private bool isShootingCooldown = false;
        private const float ShootingCooldownDuration = 0.5f;
        private float shootingCooldownTimer = 0f;

        private bool isIdling = false;
        private const float IdleTimeoutDuration = 5.0f;
        private float idleTimer = 0f;

        private int deathAnimationFrameIndex = 0;
        private int shootAnimationFrameIndex = 0;


        private Vector2 OffsetAnimation;

        private bool reachedFourthDeathFrame = false;

        // Tongue Rectangle
        public Rectangle Rectangle2;

        public Rectangle DeathRectangle;

        public AnimationHandler AnimationHandler_Minotaur;

        /*
        public override Rectangle RectangleHitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 63, 42);
            }
        }
        */

        private bool enemySpotted;

        public Rectangle EnemySpotter;

        public Vector2 EnemyPosition;

        public Rectangle TestRectangle;


        public MinotaurFix(Texture2D moveTexture, Texture2D shootTexture, Texture2D idleTexture, Texture2D deathTexture)
            : base(moveTexture)
        {
            _texture = moveTexture;
            ShootTexture = shootTexture;
            IdleTexture = idleTexture;
            DeathTexture = deathTexture;

            AnimationHandler_Minotaur = new AnimationHandler();

            Rectangle2 = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);

            facingDirectionIndicator = false;

            isIdling = true;


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
            animationShoot.fps = 1;
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
            animationDeath.fps = 8;
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

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            PositionXRectangleHitbox = (int)Position.X;
            PositionYRectangleHitbox = (int)Position.Y;
            EnemySpotter = new Rectangle((int)Position.X - 285, (int)Position.Y - 71, 354, 120);

            idleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

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

            // Shooting cooldown
            if (isShootingCooldown)
            {
                shootingCooldownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (shootingCooldownTimer >= ShootingCooldownDuration)
                {
                    isShootingCooldown = false;
                }
            }

            if (idleTimer >= IdleTimeoutDuration && !isShootingAnimating && isIdling && !isShootingCooldown)
            {
                isShootingAnimating = true;
                isIdling = false;
                if (animationShoot.IsAnimationComplete)
                {
                    idleTimer = 0;
                    isShootingAnimating = false;
                    isIdling = true;
                }
            }
            if (isShootingAnimating)
            {
                PositionXRectangleHitbox = (int)Position.X;
                PositionYRectangleHitbox = (int)Position.Y;
                WidthRectangleHitbox = 63;
                HeightRectangleHitbox = 42;
                Rectangle2.Width = 0;
                Rectangle2.Height = 0;
                shootAnimationFrameIndex = animationShoot.CurrentFrameIndex;
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
                        Rectangle2.X = (int)Position.X + 45;
                    }
                    else
                    {
                        Rectangle2.X = (int)Position.X;
                    }
                    Rectangle2.Y = (int)Position.Y - 30;
                    Rectangle2.Width = 27;
                    Rectangle2.Height = 30;
                }
                else if (shootAnimationFrameIndex == 5)
                {
                    WidthRectangleHitbox = 69;
                    HeightRectangleHitbox = 42;
                    //Tongue
                    if (facingDirectionIndicator)
                    {
                        Rectangle2.X = (int)Position.X + 63;
                    }
                    else
                    {
                        Rectangle2.X = (int)Position.X;
                    }
                    Rectangle2.Y = (int)Position.Y - 15;
                    Rectangle2.Width = 9;
                    Rectangle2.Height = 15;
                }
                else if (shootAnimationFrameIndex == 7)
                {
                    WidthRectangleHitbox = 72;
                    HeightRectangleHitbox = 42;
                }
                //RectangleHitBox.Width = 54;
                //RectangleHitBox.Height = 42;
                animationShoot.Update(gameTime);
            }

            if (!isDeathAnimating && enemySpotted)
            {
                Move();
                animationMove.Update(gameTime);
            }


            foreach (var sprite in sprites)
            {
                if (sprite is Minotaur)
                {
                    continue;
                }

                if ((sprite.RectangleHitbox.Intersects(RectangleHitbox) || sprite.RectangleHitbox.Intersects(Rectangle2)) && sprite is MainCharacter)
                {
                    HasDied = true;
                    sprite.isDeathAnimating = true;
                }

                if ((sprite.RectangleHitbox.Intersects(RectangleHitbox) || sprite.RectangleHitbox.Intersects(Rectangle2)) && sprite is PlayerBullet)
                {
                    Game1.PlayerScore.MainScore++;
                    HasDied = true;
                    isDeathAnimating = true;
                    sprite.IsRemoved = true;
                }


                if (isDeathAnimating)
                {
                    DeathRectangle = new Rectangle((int)Position.X, (int)Position.Y, 64, 64);
                    WidthRectangleHitbox = 0;
                    HeightRectangleHitbox = 0;

                    animationDeath.Update(gameTime);

                    deathAnimationFrameIndex = animationDeath.CurrentFrameIndex;

                    if (deathAnimationFrameIndex == 3) // 4th frame
                    {
                        reachedFourthDeathFrame = true;
                    }

                    if (reachedFourthDeathFrame && animationDeath.IsAnimationComplete)
                    {
                        PieceOfCodeToFall = 1;
                        IsRemoved = true;
                    }

                    if (deathAnimationFrameIndex > 6)
                    {
                        DeathRectangle.Width = 0;
                        DeathRectangle.Height = 0;
                    }

                }

                if (sprite.RectangleHitbox.Intersects(EnemySpotter) && sprite is MainCharacter)
                {
                    enemySpotted = true;
                    isIdling = false;
                }


                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is Regular_Point)
                {
                    sprite.IsRemoved = true;
                }

                if (sprite.RectangleHitbox.Intersects(DeathRectangle) && sprite is MainCharacter)
                {
                    sprite.isDeathAnimating = true;
                }
            }

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

                if (block.EnemyBehavior == true)
                {
                    TestRectangle = block.BlockRectangle;
                }
            }

            Position += Velocity;

            Velocity = Vector2.Zero;
            animationShoot.Update(gameTime);
            if (!isShootingAnimating)
            {
                animationIdle.Update(gameTime);
            }
        }

        private void Move()
        {
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
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDeathAnimating)
            {
                if (animationDeath.IsAnimationComplete)
                {
                    IsRemoved = true;
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

            spriteBatch.DrawRectangle(Rectangle2, Color.Blue);
            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
            spriteBatch.DrawRectangle(DeathRectangle, Color.Red);
            spriteBatch.DrawRectangle(EnemySpotter, Color.Yellow);
            spriteBatch.DrawRectangle(TestRectangle, Color.Red);
        }
    }
}
