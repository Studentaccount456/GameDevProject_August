using GameDevProject_August.Levels;
using Microsoft.Xna.Framework;

namespace GameDevProject_August.Interfaces
{
    public interface IBlockFactory
    {
        public Block CreateBlock(int blockType, Rectangle blockRectangle);
    }
}