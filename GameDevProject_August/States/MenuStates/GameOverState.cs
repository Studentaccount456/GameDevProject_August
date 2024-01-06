﻿using GameDevProject_August.States.LevelStates;
using GameDevProject_August.States.StateTypes;
using GameDevProject_August.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameDevProject_August.States.MenuStates
{
    internal class GameOverState : MenuState
    {
        private Texture2D backGroundGameOverState;

        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Content.ContentManager content) : base(game, graphicsDevice, content)
        {
            backGroundGameOverState = _content.Load<Texture2D>("LevelTextures\\BackGrounds\\MenuScreens\\Game_Over");
            chosenTexture = backGroundGameOverState;

            var replayButton = new Button(ButtonTexture, ButtonFont)
            {
                Position = new Vector2(Game1.ScreenWidth / 2 - ButtonTexture.Width / 2, 175),
                TextButton = "Replay",
            };

            replayButton.Click += replayButton_Click;

            var mainMenuButton = new Button(ButtonTexture, ButtonFont)
            {
                Position = new Vector2(Game1.ScreenWidth / 2 - ButtonTexture.Width / 2, 250),
                TextButton = "Main menu",
            };

            mainMenuButton.Click += mainMenuButton_Click;

            ComponentsList = new List<Component>()
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
    }
}

