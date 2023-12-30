using GameDevProject_August.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites
{
    public class Sprite
    {
        public Vector2 Position;
        public Vector2 Origin;
        public Vector2 Velocity;

        public Vector2 facingDirection = Vector2.UnitX;

        public Sprite Parent;

        private Rectangle _rectangleHitbox;

        public Sprite()
        {
            Origin = Vector2.Zero;
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {

        }

        public virtual void UpdateCollisionBlocks(GameTime gameTime, List<Block> blocks)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual Rectangle RectangleHitbox
        {
            get { return _rectangleHitbox; }
            set { _rectangleHitbox = value; }
        }

        public int PositionXRectangleHitbox
        {
            set
            {
                _rectangleHitbox.X = value;
            }
        }

        public int PositionYRectangleHitbox
        {
            set
            {
                _rectangleHitbox.Y = value;
            }
        }

        public int WidthRectangleHitbox
        {
            set
            {
                _rectangleHitbox.Width = value;
            }
        }

        public int HeightRectangleHitbox
        {
            set
            {
                _rectangleHitbox.Height = value;
            }
        }

        #region CollisionBlock

        protected bool IsTouchingLeftBlock(Block block)
        {
            return this.RectangleHitbox.Right + this.Velocity.X * 2 > block.BlockRectangle.Left &&
                   this.RectangleHitbox.Left < block.BlockRectangle.Left &&
                   this.RectangleHitbox.Bottom > block.BlockRectangle.Top &&
                   this.RectangleHitbox.Top < block.BlockRectangle.Bottom;
        }

        protected bool IsTouchingRightBlock(Block block)
        {
            return this.RectangleHitbox.Left + this.Velocity.X < block.BlockRectangle.Right &&
                   this.RectangleHitbox.Right > block.BlockRectangle.Right &&
                   this.RectangleHitbox.Bottom > block.BlockRectangle.Top &&
                   this.RectangleHitbox.Top < block.BlockRectangle.Bottom;
        }

        protected bool IsTouchingTopBlock(Block block)
        {
            return this.RectangleHitbox.Bottom + this.Velocity.Y * 2 > block.BlockRectangle.Top &&
                   this.RectangleHitbox.Top < block.BlockRectangle.Top &&
                   this.RectangleHitbox.Right > block.BlockRectangle.Left &&
                   this.RectangleHitbox.Left < block.BlockRectangle.Right;
        }

        protected bool IsTouchingBottomBlock(Block block)
        {
            return this.RectangleHitbox.Top + this.Velocity.Y < block.BlockRectangle.Bottom &&
                   this.RectangleHitbox.Bottom > block.BlockRectangle.Bottom &&
                   this.RectangleHitbox.Right > block.BlockRectangle.Left &&
                   this.RectangleHitbox.Left < block.BlockRectangle.Right;
        }
        #endregion
    }
}
