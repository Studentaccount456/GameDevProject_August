using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Sprites.NotSentient.Collectibles;
using GameDevProject_August.Sprites.NotSentient.Projectiles;
using GameDevProject_August.Sprites.Sentient.Characters.Main;
using GameDevProject_August.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject_August.Sprites.Sentient.Characters.Enemy
{
    public class RatMage : Sprite
    {
        public EnemyBullet Bullet;

        public bool HasDied = false;

        private Animation animationMove;
        private Animation animationIdle;
        private Animation animationCast;

        public Texture2D ShootTexture;
        public Texture2D IdleTexture;

        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 34, 50);
            }
        }

        public RatMage(Texture2D moveTexture, Texture2D shootTexture, Texture2D idleTexture)
            : base(moveTexture)
        {
            _texture = moveTexture;
            ShootTexture = shootTexture;
            IdleTexture = idleTexture;

            // Standard walks right
            #region MoveAnimation
            animationMove = new Animation();
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
            animationCast = new Animation();
            animationCast.AddFrame(new AnimationFrame(new Rectangle(0, 0, 60, 48)));
            animationCast.AddFrame(new AnimationFrame(new Rectangle(96, 0, 51, 48)));
            animationCast.AddFrame(new AnimationFrame(new Rectangle(192, 0, 42, 48)));
            animationCast.AddFrame(new AnimationFrame(new Rectangle(288, 0, 42, 48)));
            animationCast.AddFrame(new AnimationFrame(new Rectangle(384, 0, 72, 48)));
            animationCast.AddFrame(new AnimationFrame(new Rectangle(480, 0, 69, 48)));
            #endregion

            //Height is 44 for each frame
            #region Idle
            animationIdle = new Animation();
            animationIdle.fps = 8;
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

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            Move();

            if (_currentKey.IsKeyDown(Keys.Space) && _previousKey.IsKeyUp(Keys.Space))
            {
                AddBullet(sprites);
            }

            foreach (var sprite in sprites)
            {
                if (sprite is RatMage)
                {
                    continue;
                }

                if (sprite.Rectangle.Intersects(Rectangle) && sprite is PlayerBullet)
                {
                    HasDied = true;
                }

                if (sprite.Rectangle.Intersects(Rectangle) && sprite is Regular_Point)
                {
                    sprite.IsRemoved = true;
                }

                if (sprite is MainCharacter)
                {
                    if (Velocity.X > 0 && IsTouchingLeft(sprite) ||
                        Velocity.X < 0 && IsTouchingRight(sprite))
                    {
                        Velocity.X = 0;
                    }

                    if (Velocity.Y > 0 && IsTouchingTop(sprite) ||
                        Velocity.Y < 0 && IsTouchingBottom(sprite))
                    {
                        Velocity.Y = 0;
                    }

                }

            }

            Position += Velocity;

            Velocity = Vector2.Zero;
            animationMove.Update(gameTime);
            animationCast.Update(gameTime);
            animationIdle.Update(gameTime);
        }

        
        private void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as EnemyBullet;
            bullet.facingDirection = facingDirection;
            bullet.Position = Position + OriginBullet;
            bullet.Speed = Speed;
            bullet.Lifespan = 1f;
            bullet.Parent = this;

            sprites.Add(bullet);
        }
        

        private void Move()
        {
            if (Input == null)
                return;

            if (Keyboard.GetState().IsKeyDown((Keys)Input.Up))
            {
                Velocity.Y -= Speed;
            }
            if (Keyboard.GetState().IsKeyDown((Keys)Input.Down))
            {
                Velocity.Y += Speed;
            }
            if (Keyboard.GetState().IsKeyDown((Keys)Input.Left))
            {
                Velocity.X -= Speed;
                facingDirection = -Vector2.UnitX;
                facingDirectionIndicator = false;
            }
            if (Keyboard.GetState().IsKeyDown((Keys)Input.Right))
            {
                Velocity.X += Speed;
                facingDirection = Vector2.UnitX;
                facingDirectionIndicator = true;
            }

            Position = Vector2.Clamp(Position, new Vector2(0 - Rectangle.Width, 0 + Rectangle.Height / 2), new Vector2(Game1.ScreenWidth - Rectangle.Width, Game1.ScreenHeight - Rectangle.Height / 2));
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Keyboard.GetState().IsKeyDown((Keys)Input.Right))
            {
                spriteBatch.Draw(_texture, Position, animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (Keyboard.GetState().IsKeyDown((Keys)Input.Left))
            {
                spriteBatch.Draw(_texture, Position, animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }
            else if (Keyboard.GetState().IsKeyDown((Keys)Input.Shoot) && facingDirectionIndicator == true)
            {
                spriteBatch.Draw(ShootTexture, Position, animationCast.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (Keyboard.GetState().IsKeyDown((Keys)Input.Shoot) && facingDirectionIndicator == false)
            {
                spriteBatch.Draw(ShootTexture, Position, animationCast.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }
            /* Death animation
            else
            {
                spriteBatch.Draw(DeathTexture, Position, animationDeath.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            */

            else
            {
                spriteBatch.Draw(IdleTexture, Position, animationIdle.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }

        }

    }    
}
