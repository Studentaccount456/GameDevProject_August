using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using GameDevProject_August.UI;
using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Sprites.Sentient.Characters.Enemy;
using GameDevProject_August.Sprites.NotSentient.Projectiles;
using GameDevProject_August.Sprites.NotSentient.Collectibles;

namespace GameDevProject_August.Sprites.Sentient.Characters.Main
{
    public class MainCharacter : Sprite
    {
        // Might want to put it in a class if it's a score for entire game
        public Score Score;

        public Bullet Bullet;

        public bool HasDied = false;

        private Animation animationMoveRight;
        private Animation animationMoveLeft;
        private Animation animationDeath;
        private Animation animationIdle;
        private Animation animationShootLeft;
        private Animation animationShootRight;



        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 34, 50);
            }
        }

        public MainCharacter(Texture2D texture)
            : base(texture)
        {
            _texture = texture;
            animationMoveRight = new Animation();
            animationMoveRight.AddFrame(new AnimationFrame(new Rectangle(0, 0, 30, 50)));
            animationMoveRight.AddFrame(new AnimationFrame(new Rectangle(128, 0, 28, 50)));
            animationMoveRight.AddFrame(new AnimationFrame(new Rectangle(256, 0, 28, 50)));
            animationMoveRight.AddFrame(new AnimationFrame(new Rectangle(384, 0, 28, 50)));
            animationMoveRight.AddFrame(new AnimationFrame(new Rectangle(512, 0, 30, 50)));
            animationMoveRight.AddFrame(new AnimationFrame(new Rectangle(640, 0, 34, 50)));
            animationMoveRight.AddFrame(new AnimationFrame(new Rectangle(768, 0, 34, 50)));
            animationMoveRight.AddFrame(new AnimationFrame(new Rectangle(896, 0, 34, 50)));
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
                if (sprite is MainCharacter)
                {
                    continue;
                }

                if (sprite.Rectangle.Intersects(Rectangle) && sprite is FallingCode)
                {
                    HasDied = true;
                }

                if (sprite.Rectangle.Intersects(Rectangle) && sprite is Regular_Point)
                {
                    Score.MainScore++;
                    sprite.IsRemoved = true;
                }

                if (sprite is RatEnemy)
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
            animationMoveRight.Update(gameTime);
        }

        private void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as Bullet;
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
            }
            if (Keyboard.GetState().IsKeyDown((Keys)Input.Right))
            {
                Velocity.X += Speed;
                facingDirection = Vector2.UnitX;
            }

            Position = Vector2.Clamp(Position, new Vector2(0 - Rectangle.Width, 0 + Rectangle.Height / 2), new Vector2(Game1.ScreenWidth - Rectangle.Width, Game1.ScreenHeight - Rectangle.Height / 2));
            //Position = Vector2.Clamp(Position, new Vector2(0,0), new Vector2(Game1.ScreenWidth - this.Rectangle.Width, Game1.ScreenHeight - this.Rectangle.Height));

            //Restrict movement on x-axis

            //Position.X = MathHelper.Clamp(Position.X, 0, Game1.ScreenWidth - Rectangle.Width);
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(_texture, new Vector2(0,0), animation.CurrentFrame.SourceRectangle, Color.White);

            spriteBatch.Draw(_texture, Position, animationMoveRight.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
        }

    }
}
