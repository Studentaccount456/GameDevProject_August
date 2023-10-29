using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Levels;
using GameDevProject_August.Sprites.DNotSentient;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System.Collections.Generic;
using System.Security.Policy;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy
{
    public class Dragonfly : Enemy
    {
        private Animation animationMove;

        private bool isMovingUp = true;

        
        // Consistent Hitbox
        public override Rectangle RectangleHitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 51, 39);
            }
        }
        

        public Dragonfly(Texture2D moveTexture, Texture2D deathTexture)
            : base(moveTexture,deathTexture)
        {
            _texture = moveTexture;
            DeathTexture = deathTexture;
            facingDirectionIndicator = false;

            // Standard walks right
            #region MoveAnimation
            animationMove = new Animation(AnimationType.Move, moveTexture);
            animationMove.fps = 8;
            animationMove.AddFrame(new AnimationFrame(new Rectangle(0, 0, 51, 42)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(96, 0, 51, 42)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(192, 0, 51, 42)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(288, 0, 51, 42)));
            #endregion
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            Move(gameTime, blocks);

            CollisionRules(gameTime, sprites);

        }

        protected override void SpecificCollisionRules(Sprite sprite)
        {

        }

        private void Move(GameTime gameTime, List<Block> blocks)
        {
            foreach (var block in blocks)
            {
                if (block.BlockRectangle.Intersects(RectangleHitbox) && block.EnemyBehavior == true)
                {
                    isMovingUp = !isMovingUp;
                }
            }

            if (!isDeathAnimating)
            {
                if (isMovingUp)
                {
                    Velocity.Y -= Speed;
                }
                if (!isMovingUp)
                {
                    Velocity.Y += Speed;
                }
                Position = Vector2.Clamp(Position, new Vector2(0 - RectangleHitbox.Width, 0 + RectangleHitbox.Height / 2), new Vector2(Game1.ScreenWidth - RectangleHitbox.Width, Game1.ScreenHeight - RectangleHitbox.Height / 2));
                animationMove.Update(gameTime);
            }
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDeathAnimating)
            {
                if (reachedFourthDeathFrame)
                {
                    spriteBatch.Draw(DeathTexture, Position, animationDeath.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(DeathTexture, Position, animationDeath.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.None, 0);
                }
            }
            else if (isMovingUp || !isMovingUp)
            {
                spriteBatch.Draw(_texture, Position, animationMove.CurrentFrame.SourceRectangle, Colour, 0, Origin, 1, SpriteEffects.FlipHorizontally, 0);
            }

            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
            spriteBatch.DrawRectangle(DeathRectangle, Color.Red);
        }

        /*protected override void SpecificCollisionRules(Sprite sprite)
        {
            if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is Archeologist && sprite is Sentient sentient)
            {
                sentient.isDeathAnimating = true;
            }
                
        }*/
    }
}
