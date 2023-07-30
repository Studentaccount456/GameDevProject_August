using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameDevProject_August
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private PlayableCharacter _sprite1;
        private PlayableCharacter _sprite2;

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

            // TODO: use this.Content to load your game content here

            var texture = Content.Load<Texture2D>("Widthlook");

            _sprite1 = new PlayableCharacter(texture);
            _sprite1.Position = new Vector2(100, 100);

            _sprite2 = new PlayableCharacter(texture) 
            {
                Position = new Vector2(0,0),
                Speed = 3f,
            };
        }

        protected override void Update(GameTime gameTime)
        {
            _sprite1.Update();
            _sprite2.Update();

            //GamePad logic
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            _sprite1.Draw(_spriteBatch);
            _sprite2.Draw(_spriteBatch);

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}