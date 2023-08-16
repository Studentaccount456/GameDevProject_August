using GameDevProject_August.States;
using Microsoft.Xna.Framework;

namespace GameDevProject_August.Levels.BlockTypes
{
    public class SevenPointsType : Block
    {
        public SevenPointsType(Rectangle newrectangle) : base(newrectangle)
        {
            this.isNextLevel = true;
            blockTexture = PlayingState.SevenPointsTexture;
            this.BlockRectangle = newrectangle;
        }
    }
}
