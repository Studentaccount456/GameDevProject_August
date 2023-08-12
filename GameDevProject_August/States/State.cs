using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameDevProject_August.States
{
    public abstract class State
    {
        #region Fields

        protected Microsoft.Xna.Framework.Content.ContentManager _content;

        protected GraphicsDevice _graphicsDevice;

        protected Game1 _game;

        #endregion

        #region Methods

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public virtual void PostUpdate(GameTime gameTime) { }

        public State(Game1 game, GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            _game = game;

            _graphicsDevice = graphicsDevice;

            _content = content;
        }
            
        public abstract void Update(GameTime gameTime);

        public virtual void LoadContent(ContentManager content)
        {

        }

        #endregion
    }
}
