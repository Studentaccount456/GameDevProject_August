using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.AnimationClasses
{
    public enum AnimationType
    {
        Idle,
        Move,
        Attack,
        Death
    }
    public class Animation
    {
        public AnimationFrame CurrentFrame { get; set; }
        public List<AnimationFrame> frames;
        private int counter;
        public int fps = 12;
        public AnimationType TypeAnimation;
        public Texture2D SpriteSheetTexture;

        public int CurrentFrameIndex
        {
            get { return counter; }
        }

        public bool IsAnimationComplete
        {
            get { return counter >= frames.Count - 1; }
        }

        public Animation(AnimationType animationType, Texture2D spriteSheetTexture)
        {
            frames = new List<AnimationFrame>();
            TypeAnimation = animationType;
            SpriteSheetTexture = spriteSheetTexture;
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
