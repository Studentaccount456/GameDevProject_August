using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject_August.AnimationClasses
{
    public class Animation
    {
        public AnimationFrame CurrentFrame { get; set; }
        public List<AnimationFrame> frames;
        private int counter;
        public int fps = 12;

        public bool IsAnimationComplete
        {
            get { return counter >= frames.Count - 1; }
        }

        public Animation()
        {
            frames = new List<AnimationFrame>();
        }

        public void AddFrame(AnimationFrame frame)
        {
            frames.Add(frame);
            CurrentFrame = frames[0];
        }

        public void Reset()
        {
            counter = 0;
        }

        private double secondCounter = 0;

        public void Update(GameTime gameTime)
        {
            CurrentFrame = frames[counter];

            secondCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (secondCounter >= 1d / fps)
            {
                counter++;
                secondCounter = 0;
            }

            if (counter >= frames.Count)
            {
                counter = 0;
            }
        }
    }
}
