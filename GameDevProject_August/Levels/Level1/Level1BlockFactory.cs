using GameDevProject_August.Levels.BlockTypes;
using Microsoft.Xna.Framework;

namespace GameDevProject_August.Levels.Level1
{
    internal class Level1BlockFactory : IBlockFactory
    {
        // If Block does not get used in Level it should not be here (change when levels are all made)
        public Block CreateBlock(int number, Rectangle rectangle)
        {
            Block block = null;

            if (number == 1)
            {
                block = new Grass(rectangle);
            }
            else if (number == 2)
            {
                block = new Dirt(rectangle);
            }
            else if (number == 3)
            {
                block = new ThreePointsType(rectangle);
            }
            else if (number == 4)
            {
                block = new Grass(rectangle);
                block.EnemyBehavior = true;
            }
            else if (number == 5)
            {
                block = new InvisibleBlock(rectangle);
                block.EnemyBehavior = true;
            }
            return block;
        }
    }
}
