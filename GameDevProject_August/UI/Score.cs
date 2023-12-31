﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDevProject_August.UI
{
    public class Score
    {
        public int MainScore = 0;

        private SpriteFont _font;

        private int _screenWidth;
        private int _screenHeight;

        public Score(SpriteFont font, int ScreenWidth, int ScreenHeight)
        {
            _font = font;
            _screenWidth = ScreenWidth;
            _screenHeight = ScreenHeight;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int topPosition = (int)(_screenHeight * 0.05);
            spriteBatch.DrawString(_font, MainScore.ToString(), new Vector2(_screenWidth / 2, topPosition), Color.Red);
        }

    }
}
