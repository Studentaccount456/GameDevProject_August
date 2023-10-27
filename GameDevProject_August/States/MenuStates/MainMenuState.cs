using GameDevProject_August.Sprites;
using GameDevProject_August.States.LevelStates;
using GameDevProject_August.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameDevProject_August.States.MenuStates
{
    public class MainMenuState : State
    {
        #region Fields

        private List<Component> _components;

        private Button loadGameButton;

        private Texture2D backGroundMainMenu;

        private int levelSelect = 1;

        #endregion

        public MainMenuState(Game1 game, GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Content.ContentManager content) : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("MenuTextures\\Buttons\\Classic_Button");
            var buttonFont = _content.Load<SpriteFont>("MenuTextures\\Fonts\\ButtonFont");
            backGroundMainMenu = _content.Load<Texture2D>("LevelTextures\\BackGrounds\\MenuScreens\\Start_Screen");

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.ScreenWidth / 2 - buttonTexture.Width / 2, 250),
                TextButton = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

            loadGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.ScreenWidth / 2 - buttonTexture.Width / 2, 325),
                TextButton = "Level: Level 1",
            };

            loadGameButton.Click += LoadGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.ScreenWidth / 2 - buttonTexture.Width / 2, 400),
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
                    _game.ChangeState(new Level1State(_game, _graphicsDevice, _content, true));
                    break;
                case 2:
                    _game.ChangeState(new Level2State(_game, _graphicsDevice, _content, true));
                    break;
                case 3:
                    _game.ChangeState(new Level3State(_game, _graphicsDevice, _content, true));
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
                    loadGameButton.TextButton = "Level: Level 1";
                    break;
                case 2:
                    loadGameButton.TextButton = "Level: Level 2";
                    break;
                case 3:
                    loadGameButton.TextButton = "Level: Level 3";
                    break;
                case 4:
                    loadGameButton.TextButton = "Level: Level 1";
                    levelSelect = 1;
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

        public override void PostUpdate(GameTime gameTime, List<Sprite> _sprites)
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
