using GameDevProject_August.States;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject_August.Levels.BlockTypes
{
    internal class ThreePointsType : Block
    {
        public ThreePointsType(Rectangle newrectangle) : base(newrectangle)
        {
            this.isNextLevel = true;
            blockTexture = PlayingState.StoneTexture;
            this.BlockRectangle = newrectangle;
        }
    }
}
