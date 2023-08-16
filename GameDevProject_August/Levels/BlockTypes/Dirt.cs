using GameDevProject_August.States;
using Microsoft.Xna.Framework;

namespace GameDevProject_August.Levels.BlockTypes
{
    internal class Dirt : Block
    {
        public Dirt(Rectangle newrectangle) : base(newrectangle)
        {
            this.isNextLevel = false;
            blockTexture = PlayingState.DirtTexture;
            this.BlockRectangle = newrectangle;
        }
    }
}
