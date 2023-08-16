using GameDevProject_August.States;
using Microsoft.Xna.Framework;

namespace GameDevProject_August.Levels.BlockTypes
{
    internal class Stone : Block
    {
        public Stone(Rectangle newrectangle) : base(newrectangle)
        {
            this.isNextLevel = false;
            blockTexture = PlayingState.StoneTexture;
            this.BlockRectangle = newrectangle;
        }
    }
}
