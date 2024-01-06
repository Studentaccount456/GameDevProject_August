using GameDevProject_August.States.StateTypes;
using Microsoft.Xna.Framework;

namespace GameDevProject_August.Levels.BlockTypes
{
    internal class Grass : Block
    {
        public Grass(Rectangle newrectangle) : base(newrectangle)
        {
            this.isNextLevel = false;
            blockTexture = PlayingState.GrassTexture;
            this.BlockRectangle = newrectangle;
        }
    }
}
