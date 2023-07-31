using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject_August.Sprites
{
    public class Sprite : ICloneable
    {
        protected Texture2D _texture;
        public Vector2 Position;
        public Vector2 Origin;


        public float Speed = 2f;

        public Input Input;

        protected KeyboardState _currentKey;
        protected KeyboardState _previousKey;

        public Vector2 facingDirection = Vector2.UnitX;

        public Sprite Parent;

        public float Lifespan = 0f;

        public bool IsRemoved = false;


        public Sprite(Texture2D texture)
        {
            _texture = texture;
            Origin = new Vector2(-(_texture.Width), _texture.Height / 2);
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {

        }

        /*
        public void Update()
        {
            Move();
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
            }
            if (Keyboard.GetState().IsKeyDown((Keys)Input.Right))
            {
                Position.X += Speed;
            }
        }
        */
        
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position,null, Color.White, 0, Origin,1, SpriteEffects.None,0);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
