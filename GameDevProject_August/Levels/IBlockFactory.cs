using Microsoft.Xna.Framework;

namespace GameDevProject_August.Levels
{
    public interface IBlockFactory
    {
        public Block CreateBlock(int blockType, Rectangle blockRectangle);

    }
}