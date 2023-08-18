using GameDevProject_August.Sprites.DSentient;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject_August.Sprites.DNotSentient
{
    public class NotSentient : Sprite
    {
        protected List<Sentient> SentientsList;
        protected List<NotSentient> notSentientsList;

        public NotSentient(Texture2D texture) : base(texture)
        {
        }
    }
}
