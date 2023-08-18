using GameDevProject_August.Levels;
using GameDevProject_August.Sprites.DNotSentient;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient
{
    public class Sentient : Sprite
    {
        // Sentient
        public bool isDeathAnimating = false;

        public float Speed = 2f;

        protected List<Sentient> SentientsList;
        protected List<NotSentient> notSentientsList;

        public Sentient(Texture2D texture) : base(texture)
        {
        }
    }
}
