using GameDevProject_August.Sprites;
using GameDevProject_August.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.States.StateTypes
{
    public class MenuState : State
    {
        // ButtonTextures
        public Texture2D ButtonTexture { get; private set; }

        // Backgrounds
        protected Texture2D chosenTexture { get; set; }

        // Fonts
        public SpriteFont ButtonFont { get; private set; }

        // Components

        private List<Component> componentslist;

        public List<Component> ComponentsList
        {
            get { return componentslist; }
            set { componentslist = value; }
        }


        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            Load_Buttons(content);
            Load_Fonts(content);

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(chosenTexture, new Vector2(0, 0), Color.White);

            foreach (var component in ComponentsList)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in ComponentsList)
            {
                component.Update(gameTime);
            }
        }

        public override void PostUpdate(GameTime gameTime, List<Sprite> sprites)
        {
            //Remove sprites if not needed
        }

        private void Load_Buttons(ContentManager content)
        {
            ButtonTexture = _content.Load<Texture2D>("MenuTextures\\Buttons\\Classic_Button");
        }
        private void Load_Fonts(ContentManager content)
        {
            ButtonFont = _content.Load<SpriteFont>("MenuTextures\\Fonts\\ButtonFont");
        }
    }
}
