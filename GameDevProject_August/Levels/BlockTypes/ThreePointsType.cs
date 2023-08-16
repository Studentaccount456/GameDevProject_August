using GameDevProject_August.States;
using Microsoft.Xna.Framework;

namespace GameDevProject_August.Levels.BlockTypes
{
    internal class ThreePointsType : Block
    {
        public ThreePointsType(Rectangle newrectangle) : base(newrectangle)
        {
            this.isNextLevel = true;
            blockTexture = PlayingState.ThreePointsTexture;
            this.BlockRectangle = newrectangle;
        }
    }
}
