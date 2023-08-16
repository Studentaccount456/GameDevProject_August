using GameDevProject_August.States;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
