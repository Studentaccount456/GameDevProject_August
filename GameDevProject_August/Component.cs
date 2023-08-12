using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject_August
{
    public abstract class Component
    {
        public virtual void Draw(GameTime gametime, SpriteBatch spriteBatch) 
        {
            
        }
        

        public virtual void Update(GameTime gametime) 
        { 

        }
    }
}
