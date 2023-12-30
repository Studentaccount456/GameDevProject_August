using GameDevProject_August.AnimationClasses;
using GameDevProject_August.AnimationClasses.AnimationMethods;
using GameDevProject_August.Levels;
using GameDevProject_August.Levels.BlockTypes;
using GameDevProject_August.Models;
using GameDevProject_August.Models.Movement;
using GameDevProject_August.Sprites.DNotSentient;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Collectibles;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles.Bullets.Types;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles.FallingCode;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player.TypeOfPlayer.ShootingPlayer.Characters;
using GameDevProject_August.States;
using GameDevProject_August.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Player
{
    public enum TypePlayer
    {
        Archeologist
    }
    public class Player : Sentient
    {
        public Input Input;

        protected KeyboardState _currentKey;
        protected KeyboardState _previousKey;

        public Score Score;

        protected Animation animationMove;
        protected Animation animationDeath;
        protected Animation animationIdle;
        protected Animation animationShoot;
        protected Animation animationJump;
        protected Vector2 DeathAnimationOffset = new Vector2(0, 0);
        protected Vector2 BowDownAnimationOffset = new Vector2(0, 0);

        public Dictionary<string, Animation> animationDictionary;

        protected Texture2D AttackTexture;
        protected Texture2D IdleTexture;
        protected Texture2D DeathTexture;
        protected Texture2D StandStillTexture;
        protected Texture2D JumpTexture;
        protected Texture2D BowDownTexture;

        public AnimationHandler AnimationHandler_Player;

        protected bool isIdling = false;
        protected const float IdleTimeoutDuration = 3.0f;
        protected float idleTimer = 0f;
        protected bool standStillNoIdle = false;

        protected bool hasJumped;

        protected const float GravityAcceleration = 125;

        protected bool keyPressed;

        protected bool isCharacterMoving;

        protected Biaxial_Movement Movement = new Biaxial_Movement()
        {
            Direction_X = Direction_X.Right,
            Direction_Y = Direction_Y.None
        };

        protected bool isAttackingAnimating = false;


        public Player(Texture2D moveTexture, Texture2D attackTexture, Texture2D idleTexture, Texture2D deathTexture,
                      Texture2D standStillTexture, Texture2D jumpTexture, Texture2D bowDownTexture)
                      : base(moveTexture)
        {
            hasJumped = true;

            AnimationHandler_Player = new AnimationHandler();

            StandStillTexture = standStillTexture;

            JumpTexture = jumpTexture;

            BowDownTexture = bowDownTexture;

            animationMove = new Animation(AnimationType.Move, moveTexture);
            animationShoot = new Animation(AnimationType.Attack, attackTexture);
            animationIdle = new Animation(AnimationType.Idle, idleTexture);
            animationDeath = new Animation(AnimationType.Death, deathTexture);
            animationJump = new Animation(AnimationType.Jump, jumpTexture);

        }

        public Player()
        {

        }

        public Sprite makePlayer(Input input, TypePlayer typePlayer, Texture2D moveTexture, Texture2D attackTexture, Texture2D idleTexture,
                                 Texture2D deathTexture, Texture2D standStillTexture, Texture2D jumpTexture, Texture2D bowDownTexture,
                                 Vector2 startPosition, float speed, PlayerBullet bullet, Score score)
        {
            switch (typePlayer)
            {
                case TypePlayer.Archeologist:
                    return new Archeologist(moveTexture, attackTexture, idleTexture, deathTexture, standStillTexture, jumpTexture, bowDownTexture)
                    {
                        Input = input,
                        Position = startPosition,
                        Speed = speed,
                        Bullet = bullet,
                        Score = score,
                    }
                    ;
                default:
                    return null;
            }
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            ImplementGravity(gameTime);

            //Moving (Horizontal, Vertical (Including Jump)
            Move(gameTime, blocks);

            CollisionRules(sprites, blocks);

            IdleFunctionality(gameTime);

            DeathTriggered(gameTime);

            UpdatePositionAndResetVelocity();
        }


        protected void Move(GameTime gameTime, List<Block> blocks)
        {
            if (!isAttackingAnimating)
            {
                if (Input == null)
                    return;

                if (!Keyboard.GetState().IsKeyDown((Keys)Input.Left)
                    && !Keyboard.GetState().IsKeyDown((Keys)Input.Right)
                    && !Keyboard.GetState().IsKeyDown((Keys)Input.Up)
                    && !Keyboard.GetState().IsKeyDown((Keys)Input.Down))
                {
                    isCharacterMoving = false;
                    Movement.Direction_Y = Direction_Y.None;
                }
                else if (isDeathAnimating)
                {
                    Velocity = Vector2.Zero;
                }
                else if (Keyboard.GetState().IsKeyDown((Keys)Input.Down))
                {
                    Movement.Direction_Y = Direction_Y.Down;
                    isCharacterMoving = false;
                }
                else
                {
                    isCharacterMoving = true;

                    if (Keyboard.GetState().IsKeyDown((Keys)Input.Up))
                    {
                        Movement.Direction_Y = Direction_Y.Up;
                    }

                    if (Keyboard.GetState().IsKeyDown((Keys)Input.Up) && !hasJumped)
                    {
                        hasJumped = true;
                        Movement.Direction_Y = Direction_Y.Up;

                        if (hasJumped)
                        {
                            Velocity.Y = -20f;
                            if (Keyboard.GetState().IsKeyDown((Keys)Input.Right))
                            {
                                Velocity.X = Speed;
                            }
                            if (Keyboard.GetState().IsKeyDown((Keys)Input.Left))
                            {
                                Velocity.X = -Speed;
                            }
                        }
                    }
                    if (hasJumped)
                    {
                        foreach (var block in blocks)
                        {
                            if (IsTouchingTopBlock(block))
                            {
                                hasJumped = true;
                            }
                        }
                    }
                    if (Keyboard.GetState().IsKeyDown((Keys)Input.Left) && !hasJumped)
                    {
                        Velocity.X -= Speed;
                        facingDirection = -Vector2.UnitX;
                        Movement.Direction_X = Direction_X.Left;
                    }
                    if (Keyboard.GetState().IsKeyDown((Keys)Input.Right) && !hasJumped)
                    {
                        Velocity.X += Speed;
                        facingDirection = Vector2.UnitX;
                        Movement.Direction_X = Direction_X.Right;

                    }
                }
            }
            Position = Vector2.Clamp(Position, new Vector2(0, 0 + RectangleHitbox.Height / 4),
                       new Vector2(Game1.ScreenWidth - RectangleHitbox.Width, Game1.ScreenHeight - RectangleHitbox.Height));

            animationMove.Update(gameTime);
        }

        protected void UpdatePositionAndResetVelocity()
        {
            Position += Velocity;

            if (!hasJumped)
            {
                Velocity = Vector2.Zero;
            }
        }

        protected void CollisionRules(List<Sprite> sprites, List<Block> blocks)
        {
            foreach (var sprite in sprites)
            {
                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && (sprite is FallingCode || sprite is EnemyBullet) && sprite is NotSentient notSentient)
                {
                    isDeathAnimating = true;
                    notSentient.IsDestroyed = true;
                }

                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is Regular_Point && sprite is NotSentient notSentient2)
                {
                    Score.MainScore++;
                    PlayingState.whichCodeFalls = 5;
                    notSentient2.IsDestroyed = true;
                }
            }

            foreach (var block in blocks)
            {
                if (block is Block && block is not ThreePointsType && block is not SevenPointsType && block is not InvisibleBlock)
                {
                    if ((Velocity.X > 0 && IsTouchingLeftBlock(block) ||
                        Velocity.X < 0 && IsTouchingRightBlock(block)) && !hasJumped)
                    {
                        Velocity.X = 0;
                    }

                    if (Velocity.Y > 0 && IsTouchingTopBlock(block) && !hasJumped ||
                        Velocity.Y < 0 && IsTouchingBottomBlock(block))
                    {
                        Velocity.Y = 0;
                    }

                    if (IsTouchingTopBlock(block))
                    {
                        hasJumped = false;
                    }
                }
                else if (block is ThreePointsType)
                {
                    if (block.BlockRectangle.Intersects(RectangleHitbox) && Game1.PlayerScore.MainScore >= 3)
                    {
                        PlayingState.isNextLevelTrigger = true;
                    }
                }
                else if (block is SevenPointsType)
                {
                    if (block.BlockRectangle.Intersects(RectangleHitbox) && Game1.PlayerScore.MainScore >= 7)
                    {
                        PlayingState.isNextLevelTrigger = true;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDeathAnimating)
            {
                AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["DeathAnimation"], Position + DeathAnimationOffset, (Direction)Movement.Direction_X);

                if (animationDictionary["DeathAnimation"].IsAnimationComplete)
                {
                    HasDied = true;
                    IsKilled = true;
                }
            }

            else if (isAttackingAnimating)
            {
                AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["AttackAnimation"], Position, (Direction)Movement.Direction_X);

                if (animationDictionary["AttackAnimation"].IsAnimationComplete)
                {
                    isAttackingAnimating = false;
                }
            }

            else if (hasJumped && Movement.Direction_Y == Direction_Y.None)
            {
                AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["JumpAnimation"], Position, (Direction)Movement.Direction_X);
            }

            else if (isCharacterMoving == true)
            {
                AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["MoveAnimation"], Position, (Direction)Movement.Direction_X);
            }

            else if (isIdling)
            {
                if (isCharacterMoving == false)
                {
                    AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["IdleAnimation"], Position, (Direction)Movement.Direction_X);
                }
            }

            else if (standStillNoIdle == true && Movement.Direction_Y != Direction_Y.Down)
            {
                AnimationHandler_Player.DrawOneFrameAnimation(spriteBatch, StandStillTexture, Position, (Direction)Movement.Direction_X);
            }

            else if (Movement.Direction_Y == Direction_Y.Down && !hasJumped)
            {
                AnimationHandler_Player.DrawOneFrameAnimation(spriteBatch, BowDownTexture, Position + BowDownAnimationOffset, (Direction)Movement.Direction_X);
            }

            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
        }


        protected void IdleFunctionality(GameTime gameTime)
        {
            keyPressed = Keyboard.GetState().IsKeyDown((Keys)Input.Left) || Keyboard.GetState().IsKeyDown((Keys)Input.Right) ||
                  Keyboard.GetState().IsKeyDown((Keys)Input.Up) || Keyboard.GetState().IsKeyDown((Keys)Input.Down) ||
                  Keyboard.GetState().IsKeyDown((Keys)Input.Shoot) && isAttackingAnimating;

            if (keyPressed)
            {
                idleTimer = 0f;
                isIdling = false;
                standStillNoIdle = false;

            }
            else
            {
                idleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (idleTimer >= IdleTimeoutDuration)
                {
                    isIdling = true;
                    standStillNoIdle = false;
                    animationIdle.Update(gameTime);
                }
                else
                {
                    standStillNoIdle = true;
                }
            }
        }


        protected void ImplementGravity(GameTime gameTime)
        {
            Velocity.Y += GravityAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected void DeathTriggered(GameTime gameTime)
        {
            if (isDeathAnimating == true)
            {
                animationDeath.Update(gameTime);
            }
        }
    }
}
