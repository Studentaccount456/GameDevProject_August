using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameDevProject_August.Controls
{
    public class Button : Component
    {
        #region Fields

        private MouseState _currentMouse;

        private SpriteFont _font;

        private bool _isHovering;

        private MouseState _previousMouse;

        private Texture2D _texture;

        #endregion

        #region Properties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColour { get; set; }

        public Vector2 Position { get; set; }

        public Rectangle RectangleButton 
        {
            get 
            { 
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            } 
        }

        public string TextButton { get; set; }

        #endregion

        #region Methods

        public Button(Texture2D texture,SpriteFont font)
        {
            _texture = texture;
            _font = font;

            PenColour = Color.Black;
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            var colour = Color.White;

            if(_isHovering )
            {
                colour = Color.Gray;
            }

            spriteBatch.Draw(_texture, RectangleButton, colour);

            if(!string.IsNullOrEmpty(TextButton))
            {
                var x = (RectangleButton.X + (RectangleButton.Width / 2)) - (_font.MeasureString(TextButton).X / 2);
                var y = (RectangleButton.Y + (RectangleButton.Height / 2)) - (_font.MeasureString(TextButton).Y / 2);

                spriteBatch.DrawString(_font, TextButton, new Vector2(x,y), PenColour);
            }
        }

        public override void Update(GameTime gametime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if(mouseRectangle.Intersects(RectangleButton))
            {
                _isHovering = true;

                if(_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        #endregion
    }
}
