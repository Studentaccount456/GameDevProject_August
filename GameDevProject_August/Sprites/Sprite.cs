using GameDevProject_August.Levels;
using GameDevProject_August.Models;
using GameDevProject_August.Sprites.DNotSentient;
using GameDevProject_August.Sprites.DSentient;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites
{
    public class Sprite
    {
        protected Texture2D _texture;
        public Vector2 Position;
        public Vector2 Origin;
        // Shooting
        public Vector2 OriginBullet;
        // Moving
        public Vector2 Velocity;

        //Dying -> EnemyDeath
        public int PieceOfCodeToFall;

        //Player
        public Input Input;

        // ManualMovement
        protected KeyboardState _currentKey;
        protected KeyboardState _previousKey;

        // False is left en Right is True
        public bool facingDirectionIndicator = true;
        public Vector2 facingDirection = Vector2.UnitX;

        //Shooting
        public Sprite Parent;

        // Color.Black for glitch
        public Color Colour = Color.White;

        // Collision
        private Rectangle _rectangleHitbox;

        public virtual Rectangle RectangleHitbox
        {
            get { return _rectangleHitbox; }
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
        // End Collision

        public Sprite(Texture2D texture)
        {
            _texture = texture;
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
            spriteBatch.Draw(_texture, Position, null, Colour, 0, Origin, 1, SpriteEffects.None, 0);
        }

        //Collision
        #region Collision

        protected bool IsTouchingLeft(Sprite sprite)
        {
            return this.RectangleHitbox.Right + this.Velocity.X > sprite.RectangleHitbox.Left &&
                   this.RectangleHitbox.Left < sprite.RectangleHitbox.Left &&
                   this.RectangleHitbox.Bottom > sprite.RectangleHitbox.Top &&
                   this.RectangleHitbox.Top < sprite.RectangleHitbox.Bottom;
        }

        protected bool IsTouchingRight(Sprite sprite)
        {
            return this.RectangleHitbox.Left + this.Velocity.X < sprite.RectangleHitbox.Right &&
                   this.RectangleHitbox.Right > sprite.RectangleHitbox.Right &&
                   this.RectangleHitbox.Bottom > sprite.RectangleHitbox.Top &&
                   this.RectangleHitbox.Top < sprite.RectangleHitbox.Bottom;
        }

        protected bool IsTouchingTop(Sprite sprite)
        {
            return this.RectangleHitbox.Bottom + this.Velocity.Y > sprite.RectangleHitbox.Top &&
                   this.RectangleHitbox.Top < sprite.RectangleHitbox.Top &&
                   this.RectangleHitbox.Right > sprite.RectangleHitbox.Left &&
                   this.RectangleHitbox.Left < sprite.RectangleHitbox.Right;
        }

        protected bool IsTouchingBottom(Sprite sprite)
        {
            return this.RectangleHitbox.Top + this.Velocity.Y < sprite.RectangleHitbox.Bottom &&
                   this.RectangleHitbox.Bottom > sprite.RectangleHitbox.Bottom &&
                   this.RectangleHitbox.Right > sprite.RectangleHitbox.Left &&
                   this.RectangleHitbox.Left < sprite.RectangleHitbox.Right;
        }


        #endregion

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
        //Collision
    }
}
