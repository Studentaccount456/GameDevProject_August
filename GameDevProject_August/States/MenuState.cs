using GameDevProject_August.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameDevProject_August.States
{
    public class MenuState : State
    {
        #region Fields

        private List<Component> _components;

        private int levelSelect = 1;

        private Button loadGameButton;

        private Texture2D backGroundMainMenu;

        #endregion

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Content.ContentManager content) : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("Controls\\Illustration2");
            var buttonFont = _content.Load<SpriteFont>("Fonts\\ButtonFont");
            backGroundMainMenu = _content.Load<Texture2D>("BackGrounds\\Start_Screen");

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((Game1.ScreenWidth / 2) - (buttonTexture.Width / 2), 250),
                TextButton = "New Game", 
            };

            newGameButton.Click += NewGameButton_Click;

            loadGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((Game1.ScreenWidth / 2) - (buttonTexture.Width / 2), 325),
                TextButton = "Level: Level 1",
            };

            loadGameButton.Click += LoadGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((Game1.ScreenWidth / 2) - (buttonTexture.Width / 2), 400),
                TextButton = "Quit",
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Component>()
            {
                newGameButton,
                loadGameButton,
                quitGameButton
            };
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            switch (levelSelect)
            {
                case 1:
                    _game.ChangeState(new Level1State(_game, _graphicsDevice, _content));
                    break;
                case 2:
                    _game.ChangeState(new Level2State(_game, _graphicsDevice, _content));
                    break;
                case 3:
                    _game.ChangeState(new Level2State(_game, _graphicsDevice, _content));
                    break;
                default:
                    break;
            }
        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            levelSelect++;
            switch (levelSelect)
            {
                case 1:
                    loadGameButton.TextButton =  "Level: Level 1";
                    break;
                case 2:
                    loadGameButton.TextButton = "Level: Level 2";
                    break;
                case 3:
                    loadGameButton.TextButton = "Level: Level 3";
                    levelSelect = 0;
                    break;
                default:
                    break;
            }
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(backGroundMainMenu, new Vector2(0, 0), Color.White);

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
