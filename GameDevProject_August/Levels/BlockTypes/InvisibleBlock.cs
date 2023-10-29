using GameDevProject_August.States;
using Microsoft.Xna.Framework;

namespace GameDevProject_August.Levels.BlockTypes
{
    internal class InvisibleBlock : Block
    {
        public InvisibleBlock(Rectangle newrectangle) : base(newrectangle)
        {
                this.isNextLevel = false;
                blockTexture = PlayingState.InvisibleBlockTexture;
                this.BlockRectangle = newrectangle;
        }
    }
}
