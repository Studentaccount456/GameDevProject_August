using GameDevProject_August.States.LevelStates;
using GameDevProject_August.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameDevProject_August.States.MenuStates
{
    internal class GameOverState : State
    {
        #region Fields

        private List<Component> _components;

        private Texture2D backGroundGameOverState;

        #endregion

        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Content.ContentManager content) : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("Controls\\Illustration2");
            backGroundGameOverState = _content.Load<Texture2D>("BackGrounds\\Game_Over");
            var buttonFont = _content.Load<SpriteFont>("Fonts\\ButtonFont");

            var replayButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.ScreenWidth / 2 - buttonTexture.Width / 2, 175),
                TextButton = "Replay",
            };

            replayButton.Click += replayButton_Click;

            var mainMenuButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.ScreenWidth / 2 - buttonTexture.Width / 2, 250),
                TextButton = "Main menu",
            };

            mainMenuButton.Click += mainMenuButton_Click;

            _components = new List<Component>()
            {
                replayButton,
                mainMenuButton
            };
        }

        private void replayButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new Level1State(_game, _graphicsDevice, _content, true));
        }

        private void mainMenuButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MainMenuState(_game, _graphicsDevice, _content));
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(backGroundGameOverState, new Vector2(0, 0), Color.White);

            foreach (var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //Remove sprites if not needed
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }
    }
}

