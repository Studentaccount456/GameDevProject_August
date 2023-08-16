using GameDevProject_August.Levels.BlockTypes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject_August.Levels.Level2
{
    internal class Level2BlockFactory : IBlockFactory
    {
        public Block CreateBlock(int number, Rectangle rectangle)
        {
            Block block = null;

            if (number == 1)
            {
                block = new Stone(rectangle);
            }
            else if (number == 2)
            {
                block = new SevenPointsType(rectangle);
            } else if (number == 3)
            {
                block = new Stone(rectangle);
                block.EnemyBehavior = true;
            } else if (number == 4)
            {
                block = new Stone(rectangle);
                block.EnemyBehavior_2 = true;
            }
            return block;
        }
    }
}
