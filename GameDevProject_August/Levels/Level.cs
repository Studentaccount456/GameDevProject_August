using GameDevProject_August.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Levels
{
    abstract public class Level
    {
        #region Fields

        private List<Block> tileList = new List<Block>();

        private int widthLevel;
        private int heightLevel;

        private int[,] map;

        private IBlockFactory blockFactory;

        #endregion

        #region Properties

        public List<Block> TileList
        {
            get { return tileList; }
        }

        public int Width { get { return widthLevel; } }
        public int Height { get { return heightLevel; } }

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

                    widthLevel = (x + 1) * size;
                    heightLevel = (y + 1) * size;
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