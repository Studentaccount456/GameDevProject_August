using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Levels
{
    abstract public class Level
    {
        #region Fields

        private List<Block> tileList = new List<Block>();

        //Width/height of level
        private int width;
        private int height;

        private int[,] map;

        private IBlockFactory blockFactory;

        #endregion

        #region Properties

        public List<Block> TileList
        {
            get { return tileList; }
        }

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public int[,] Map { get { return map; } set { map = value; } }

        #endregion

        public Level(IBlockFactory blockFactory)
        {
            this.blockFactory = blockFactory;
        }

        #region Methods

        public void Generate(int[,] map, int size)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    int number = map[y, x];

                    if (number > 0)
                    {
                        tileList.Add(blockFactory.CreateBlock(number, new Rectangle(x * size, y * size, size, size)));
                    }

                    width = (x + 1) * size;
                    height = (y + 1) * size;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Block tile in tileList)
            {
                tile.Draw(spriteBatch);
            }
        }

        #endregion
    }
}