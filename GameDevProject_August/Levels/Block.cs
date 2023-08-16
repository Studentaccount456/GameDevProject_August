using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDevProject_August.Levels
{
    abstract public class Block
    {
        #region fields

        protected Texture2D blockTexture;

        private Rectangle blockRectangle;

        // Will be used for doorBlock that will allow you to move to next Level.
        public bool isNextLevel;

        public bool EnemyBehavior = false;

        #endregion

        #region Properties

        public Rectangle BlockRectangle
        {
            get { return blockRectangle; }
            set { blockRectangle = value; }
        }

        public Block(Rectangle newrectangle)
        {
            this.BlockRectangle = newrectangle;
            this.isNextLevel = false;
        }

        #endregion

        #region Methods

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(blockTexture, blockRectangle, Color.White);
        }

        #endregion
    }
}
