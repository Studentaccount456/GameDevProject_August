using GameDevProject_August.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy.AttackingEnemy.SpotterEnemy.MeleeEnemy
{
    abstract public class MeleeEnemy : SpotterEnemy
    {
        protected int meleeAttackAnimationFrameIndex = 0;

        protected bool _enemySpotted;
        public Vector2 EnemyPosition;
        protected bool canSeeEnemy = true;

        public MeleeEnemy(Texture2D moveTexture, Texture2D attackTexture, Texture2D deathTexture, Vector2 StartPosition, Vector2 offsetPositionSpotter, int widthSpotter, int heightSpotter) : base(moveTexture, attackTexture, deathTexture, StartPosition, offsetPositionSpotter, widthSpotter, heightSpotter)
        {

        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            base.Update(gameTime, sprites, blocks);

            MeleeAttackImplementation(gameTime);
        }

        protected void MeleeAttackImplementation(GameTime gameTime)
        {
            animationAttack.Update(gameTime);

            UniqueMeleeAttackImplementation(gameTime);

        }

        protected abstract void UniqueMeleeAttackImplementation(GameTime gameTime);

        protected override void UniqueMovingRules(GameTime gameTime, List<Block> blocks)
        {
            throw new NotImplementedException();
        }

        protected override void UniqueCollisionRules(Sprite sprite, Rectangle hitbox, bool isHardSpot)
        {
            throw new NotImplementedException();
        }
    }
}
