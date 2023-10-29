using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles;
using GameDevProject_August.Sprites.DNotSentient;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy
{
    abstract public class Enemy : Sentient
    {
        protected Texture2D DeathTexture;

        protected Animation animationDeath;

        public Rectangle DeathRectangle;

        protected int deathAnimationFrameIndex = 0;

        protected bool reachedFourthDeathFrame = false;

        protected List<Rectangle> hitboxes;


        public Enemy(Texture2D texture, Texture2D deathTexture) : base(texture)
        {
            //Height is 44 for each frame
            #region Death
            animationDeath = new Animation(AnimationType.Death, deathTexture);
            animationDeath.fps = 4;
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(0, 0, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(64, 0, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(128, 0, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(192, 0, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(0, 64, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(64, 64, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(128, 64, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(192, 64, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(0, 128, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(64, 128, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(128, 128, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(192, 128, 64, 64)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(0, 192, 64, 64)));
            #endregion
        }

        protected void GlitchDeathInit(GameTime gameTime, Sprite sprite, int pieceOfCodeToFall)
        {
            if (isDeathAnimating)
            {
                DeathRectangle = new Rectangle((int)Position.X, (int)Position.Y, 64, 64);

                animationDeath.Update(gameTime);

                deathAnimationFrameIndex = animationDeath.CurrentFrameIndex;

                if (deathAnimationFrameIndex == 3) // 4th frame
                {
                    reachedFourthDeathFrame = true;
                }

                if (reachedFourthDeathFrame && animationDeath.IsAnimationComplete)
                {
                    PieceOfCodeToFall = pieceOfCodeToFall;
                    IsKilled = true;
                }

                if (sprite.RectangleHitbox.Intersects(DeathRectangle) && sprite is Archeologist && sprite is Sentient sentient)
                {
                    sentient.isDeathAnimating = true;
                }

                WidthRectangleHitbox = 0;
                HeightRectangleHitbox = 0;

                if (deathAnimationFrameIndex > 6)
                {
                    DeathRectangle.Width = 0;
                    DeathRectangle.Height = 0;
                }

            }
        }

        protected void UpdatePositionAndResetVelocity()
        {
            Position += Velocity;

            Velocity = Vector2.Zero;
        }

        protected virtual void CollisionRules(GameTime gameTime, List<Sprite> sprites)
        {
            foreach (var sprite in sprites)
            {
                //foreach (var hitbox in hitboxes)
                {
                    if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is PlayerBullet && sprite is NotSentient notSentient)
                    {
                        Game1.PlayerScore.MainScore++;
                        isDeathAnimating = true;
                        notSentient.IsDestroyed = true;
                    }
                    if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is Archeologist && sprite is Sentient sentient)
                    {
                        sentient.isDeathAnimating = true;
                    }
                }

                SpecificCollisionRules(sprite);

                GlitchDeathInit(gameTime, sprite, 2);

                UpdatePositionAndResetVelocity();
            }
        }

        protected abstract void SpecificCollisionRules(Sprite sprite);
    }


}
