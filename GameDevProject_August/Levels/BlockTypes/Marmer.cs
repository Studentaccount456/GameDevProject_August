using GameDevProject_August.States.StateTypes;
using Microsoft.Xna.Framework;

namespace GameDevProject_August.Levels.BlockTypes
{
    internal class Marmer : Block
    {
        public Marmer(Rectangle newrectangle) : base(newrectangle)
        {
            this.isNextLevel = false;
            blockTexture = PlayingState.MarmerTexture;
            this.BlockRectangle = newrectangle;
        }
    }
}
