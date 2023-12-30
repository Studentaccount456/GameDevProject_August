using GameDevProject_August.Interfaces;
using GameDevProject_August.Levels.BlockTypes;
using Microsoft.Xna.Framework;

namespace GameDevProject_August.Levels.Level3
{
    internal class Level3BlockFactory : IBlockFactory
    {
        public Block CreateBlock(int number, Rectangle rectangle)
        {
            Block block = null;

            if (number == 1)
            {
                block = new Marmer(rectangle);
            }
            return block;
        }
    }
}
