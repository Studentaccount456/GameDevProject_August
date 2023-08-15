﻿using GameDevProject_August.Levels;
using GameDevProject_August.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites
{
    public class Sprite : Component
    {
        protected Texture2D _texture;
        public Vector2 Position;
        public Vector2 Origin;
        public Vector2 OriginBullet;
        public Vector2 Velocity;

        public float Speed = 2f;

        public Input Input;

        protected KeyboardState _currentKey;
        protected KeyboardState _previousKey;

        public Vector2 facingDirection = Vector2.UnitX;
        // False is left en Right is True
        public bool facingDirectionIndicator = true;

        public Sprite Parent;

        public bool IsRemoved = false;

        // Color.Black for glitch
        public Color Colour = Color.White;

        public virtual Rectangle Rectangle
        {
            get 
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public Sprite(Texture2D texture)
        {
            _texture = texture;
            Origin = Vector2.Zero;
            OriginBullet = new Vector2(0, _texture.Height / 2);
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

        #region Collision

        protected bool IsTouchingLeft(Sprite sprite)
        {
            return this.Rectangle.Right + this.Velocity.X > sprite.Rectangle.Left &&
                   this.Rectangle.Left < sprite.Rectangle.Left &&
                   this.Rectangle.Bottom > sprite.Rectangle.Top &&
                   this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingRight(Sprite sprite)
        {
            return this.Rectangle.Left + this.Velocity.X < sprite.Rectangle.Right &&
                   this.Rectangle.Right > sprite.Rectangle.Right &&
                   this.Rectangle.Bottom > sprite.Rectangle.Top &&
                   this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingTop(Sprite sprite)
        {
            return this.Rectangle.Bottom + this.Velocity.Y > sprite.Rectangle.Top &&
                   this.Rectangle.Top < sprite.Rectangle.Top &&
                   this.Rectangle.Right > sprite.Rectangle.Left &&
                   this.Rectangle.Left < sprite.Rectangle.Right;
        }

        protected bool IsTouchingBottom(Sprite sprite)
        {
            return this.Rectangle.Top + this.Velocity.Y < sprite.Rectangle.Bottom &&
                   this.Rectangle.Bottom > sprite.Rectangle.Bottom &&
                   this.Rectangle.Right > sprite.Rectangle.Left &&
                   this.Rectangle.Left < sprite.Rectangle.Right;
        }


        #endregion

        #region CollisionBlock

        protected bool IsTouchingLeftBlock(Block block)
        {
            return this.Rectangle.Right + this.Velocity.X > block.BlockRectangle.Left &&
                   this.Rectangle.Left < block.BlockRectangle.Left &&
                   this.Rectangle.Bottom > block.BlockRectangle.Top &&
                   this.Rectangle.Top < block.BlockRectangle.Bottom;
        }

        protected bool IsTouchingRightBlock(Block block)
        {
            return this.Rectangle.Left + this.Velocity.X < block.BlockRectangle.Right &&
                   this.Rectangle.Right > block.BlockRectangle.Right &&
                   this.Rectangle.Bottom > block.BlockRectangle.Top &&
                   this.Rectangle.Top < block.BlockRectangle.Bottom;
        }

        protected bool IsTouchingTopBlock(Block block)
        {
            return this.Rectangle.Bottom + this.Velocity.Y > block.BlockRectangle.Top &&
                   this.Rectangle.Top < block.BlockRectangle.Top &&
                   this.Rectangle.Right > block.BlockRectangle.Left &&
                   this.Rectangle.Left < block.BlockRectangle.Right;
        }

        protected bool IsTouchingBottomBlock(Block block)
        {
            return this.Rectangle.Top + this.Velocity.Y < block.BlockRectangle.Bottom &&
                   this.Rectangle.Bottom > block.BlockRectangle.Bottom &&
                   this.Rectangle.Right > block.BlockRectangle.Left &&
                   this.Rectangle.Left < block.BlockRectangle.Right;
        }


        #endregion
    }
}
