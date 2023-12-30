using GameDevProject_August.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy.SpotterEnemy
{
    public class SpotterEnemy : Enemy
    {
        protected bool isAttackCooldown = false;
        protected const float AttackCooldownDuration = 1f;
        protected float AttackCooldownTimer = 0f;

        protected int _widthSpotter, _heightSpotter;
        protected Vector2 _offsetPositonSpotter;

        public Rectangle EnemySpotter;


        public SpotterEnemy(Texture2D moveTexture, Texture2D deathTexture, Vector2 StartPosition, Vector2 offsetPositionSpotter, int widthSpotter, int heightSpotter) : base(moveTexture, deathTexture, StartPosition)
        {
            _offsetPositonSpotter = offsetPositionSpotter;
            _widthSpotter = widthSpotter;
            _heightSpotter = heightSpotter;

            InitializeEnemySpotter(Position, _offsetPositonSpotter, _widthSpotter, _heightSpotter);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            base.Update(gameTime, sprites, blocks);

            InitializeEnemySpotter(Position, _offsetPositonSpotter, _widthSpotter, _heightSpotter);
        }

        protected void AttackCooldown(GameTime gameTime)
        {
            // Attack cooldown
            if (isAttackCooldown)
            {
                AttackCooldownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (AttackCooldownTimer >= AttackCooldownDuration)
                {
                    isAttackCooldown = false;
                }
            }
        }

        protected void InitializeEnemySpotter(Vector2 position, Vector2 offsetPositionSpotter, int widthSpotter, int heightSpotter)
        {
            // Initialize in Constructor + use RemoveEnemySpotterSpotted() when spotter needs dissapear after spot
            EnemySpotter = new Rectangle((int)(position.X - offsetPositionSpotter.X), (int)(position.Y - offsetPositionSpotter.Y), widthSpotter, heightSpotter);
            // Otherwise put in update so the Position updates so the spot can be reset
        }

        protected override void UniqueCollisionRules(Sprite sprite, Rectangle hitbox, bool isHardSpot)
        {
            throw new NotImplementedException();
        }

        protected override void UniqueDrawRules(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        protected override void UniqueMovingRules(GameTime gameTime, List<Block> blocks)
        {
            throw new NotImplementedException();
        }

        protected override void HitBoxTracker()
        {
            throw new NotImplementedException();
        }
    }
}
