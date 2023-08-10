using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Models;
using GameDevProject_August.Sprites.NotSentient.Collectibles;
using GameDevProject_August.Sprites.NotSentient.Projectiles;
using GameDevProject_August.Sprites.Sentient.Characters.Main;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace GameDevProject_August.Sprites.Sentient.Characters.Enemy
{
    public class Minotaur : Sprite
    {
        //Integrate melee attack

        public bool HasDied = false;

        private Animation animationMove;
        private Animation animationIdle;
        private Animation animationShoot;

        public Texture2D ShootTexture;
        public Texture2D IdleExture;
        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 34, 50);
            }
        }

        public Minotaur(Texture2D moveTexture, Texture2D shootTexture, Texture2D idleTexture)
            : base(moveTexture)
        {
            _texture = moveTexture;
            ShootTexture = shootTexture;
            IdleExture = idleTexture;

            // Difference off 128 x
            // Standard walks right
            #region MoveAnimation
            animationMove = new Animation();
            animationMove.AddFrame(new AnimationFrame(new Rectangle(0, 0, 30, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(128, 0, 28, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(256, 0, 28, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(384, 0, 28, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(512, 0, 30, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(640, 0, 34, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(768, 0, 34, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(896, 0, 34, 50)));
            #endregion

            //Height is 48 for each frame
            #region animationShoot
            animationShoot = new Animation();
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(0, 0, 36, 50)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(126, 0, 44, 50)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(256, 0, 40, 50)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(384, 0, 44, 50)));
            #endregion

            //Height is 44 for each frame
            #region Idle
            animationIdle = new Animation();
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(0, 0, 32, 50)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(128, 0, 32, 50)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(256, 0, 32, 50)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(384, 0, 32, 50)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(512, 0, 32, 50)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(640, 0, 32, 50)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(768, 0, 32, 50)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(896, 0, 32, 50)));
            #endregion

        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            Move();

            /*
            if (_currentKey.IsKeyDown(Keys.Space) && _previousKey.IsKeyUp(Keys.Space))
            {
                //AddBullet(sprites);
            }
            */

            foreach (var sprite in sprites)
            {
                if (sprite is Minotaur)
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
            animationShoot.Update(gameTime);
            animationIdle.Update(gameTime);
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
                spriteBatch.Draw(ShootTexture, Position, animationShoot.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            else if (Keyboard.GetState().IsKeyDown((Keys)Input.Shoot) && facingDirectionIndicator == false)
            {
                spriteBatch.Draw(ShootTexture, Position, animationShoot.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }
            /* Death animation
            else
            {
                spriteBatch.Draw(DeathTexture, Position, animationDeath.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }
            */

            else
            {
                spriteBatch.Draw(IdleExture, Position, animationIdle.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
            }

        }

    }
}
