using GameDevProject_August.States.StateTypes;
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
