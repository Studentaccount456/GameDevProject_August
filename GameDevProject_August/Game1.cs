using GameDevProject_August.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameDevProject_August
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        /*private Sprite _sprite1;
        private Sprite _sprite2;*/

        private List<Sprite> _sprites;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var personTexture = Content.Load<Texture2D>("Widthlook");

            _sprites = new List<Sprite>()
            {
                new MainCharacter(personTexture)
                {
                    Position = new Vector2(100,100),
                    Input = new Input()
                    {
                        Down = System.Windows.Forms.Keys.Down,
                        Up = System.Windows.Forms.Keys.Up,
                        Left = System.Windows.Forms.Keys.Left,
                        Right = System.Windows.Forms.Keys.Right
                    },
                    Bullet = new Bullet(Content.Load<Texture2D>("GoToeBullet"))
                },

                new MainCharacter(personTexture)                
                {
                    Position = new Vector2(200,100),
                    Input = new Input()
                    {
                        Down = System.Windows.Forms.Keys.S,
                        Up = System.Windows.Forms.Keys.Z,
                        Left = System.Windows.Forms.Keys.Q,
                        Right = System.Windows.Forms.Keys.D
                    },
                    Bullet = new Bullet(Content.Load<Texture2D>("GoToeBullet"))

                },
            };

            /*_sprite1 = new Sprite(texture);
            _sprite1.Position = new Vector2(100, 100);

            _sprite2 = new Sprite(texture) 
            {
                Position = new Vector2(0,0),
                Speed = 3f,
            }; */
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (var sprite in _sprites.ToArray())
            {
                sprite.Update(gameTime, _sprites);
            }

            PostUpdate();
            /*_sprite1.Update();
            _sprite2.Update();*/

            //GamePad logic
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void PostUpdate()
        {
            for (int i = 0; i < _sprites.Count; i++)
            {
                if (_sprites[i].IsRemoved)
                {
                    _sprites.RemoveAt(i);
                    i--;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            foreach (var sprite in _sprites)
            {
                sprite.Draw(_spriteBatch);
            }

            /*_sprite1.Draw(_spriteBatch);
            _sprite2.Draw(_spriteBatch);*/

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}