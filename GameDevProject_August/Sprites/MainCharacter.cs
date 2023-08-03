using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace GameDevProject_August.Sprites
{
    public class MainCharacter : Sprite
    {
        public Bullet Bullet;

        public bool HasDied = false;

        public MainCharacter(Texture2D texture) 
            : base(texture)
        {
            
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
                if(sprite is MainCharacter) 
                {
                    continue;
                }

                if(sprite.Rectangle.Intersects(Rectangle) && sprite is FallingCode)
                {
                    this.HasDied = true;
                }
            }

            Position.X = MathHelper.Clamp(Position.X, 0, Game1.ScreenWidth - Rectangle.Width);

        }

        private void AddBullet(List<Sprite> sprites)
        {
            var bullet = Bullet.Clone() as Bullet;
            bullet.facingDirection = this.facingDirection;
            bullet.Position = this.Position;
            bullet.Speed = this.Speed * 2;
            bullet.Lifespan = 2f;
            bullet.Parent = this;

            sprites.Add(bullet);
        }

        private void Move()
        {
            if (Input == null)
                return;

            if (Keyboard.GetState().IsKeyDown((Keys)Input.Up))
            {
                Position.Y -= Speed;
            }
            if (Keyboard.GetState().IsKeyDown((Keys)Input.Down))
            {
                Position.Y += Speed;
            }
            if (Keyboard.GetState().IsKeyDown((Keys)Input.Left))
            {
                Position.X -= Speed;
                facingDirection = -Vector2.UnitX;
            }
            if (Keyboard.GetState().IsKeyDown((Keys)Input.Right))
            {
                Position.X += Speed;
                facingDirection = Vector2.UnitX;
            }
        }
    }
}
